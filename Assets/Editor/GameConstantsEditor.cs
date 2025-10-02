using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(GameConstants))]
public class GameConstantsEditor : Editor
{
    private Dictionary<string, List<ConstantEntry>> groupedConstants;

    public override void OnInspectorGUI()
    {
        var constants = (GameConstants)target;

        if (groupedConstants == null)
            GroupConstants(constants);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Game Constants", EditorStyles.boldLabel);
        GUILayout.Space(5);

        // Показываем сгруппированные константы
        foreach (var group in groupedConstants)
        {
            EditorGUILayout.LabelField(group.Key, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            foreach (var constant in group.Value)
            {
                var fieldInfo = typeof(ConstantType).GetField(constant.type.ToString());
                var attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;

                var content = new GUIContent(constant.type.ToString(),
                    attribute?.Description ?? "No description");

                // Сохраняем старое значение для проверки изменений
                var oldValue = constant.value;
                constant.value = EditorGUILayout.FloatField(content, constant.value);

                // Показываем значение по умолчанию если отличается
                if (Mathf.Abs(constant.value - (attribute?.DefaultValue ?? 0f)) > 0.01f)
                {
                    EditorGUILayout.LabelField($"Default: {attribute?.DefaultValue}", EditorStyles.miniLabel);
                }

                // Показываем предупреждение для "сиротских" констант
                if (!IsValidConstantType(constant.type))
                {
                    EditorGUILayout.HelpBox($"Constant type '{constant.type}' no longer exists in enum!", MessageType.Warning);
                }
            }

            EditorGUI.indentLevel--;
            GUILayout.Space(10);
        }

        // Кнопки управления
        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Initialize Constants"))
        {
            constants.Initialize();
            EditorUtility.SetDirty(constants);
            GroupConstants(constants);
            Debug.Log("Constants initialized!");
        }

        if (GUILayout.Button("Add Missing"))
        {
            AddMissingConstants(constants);
            GroupConstants(constants);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Remove Orphaned", GUILayout.Width(150)))
        {
            if (EditorUtility.DisplayDialog("Remove Orphaned Constants",
                "This will remove all constants that no longer exist in the ConstantType enum. Continue?",
                "Remove", "Cancel"))
            {
                int removedCount = RemoveOrphanedConstants(constants);
                GroupConstants(constants);
                Debug.Log($"Removed {removedCount} orphaned constants");
            }
        }

        if (GUILayout.Button("Reset to Defaults", GUILayout.Width(150)))
        {
            if (EditorUtility.DisplayDialog("Reset to Defaults",
                "Reset ALL constants to their default values?",
                "Reset", "Cancel"))
            {
                ResetToDefaults(constants);
                GroupConstants(constants);
                Debug.Log("All constants reset to defaults");
            }
        }

        EditorGUILayout.EndHorizontal();

        // Статистика
        GUILayout.Space(10);
        EditorGUILayout.LabelField($"Total Constants: {GetTotalConstantsCount(constants)}", EditorStyles.miniLabel);
        EditorGUILayout.LabelField($"Orphaned Constants: {GetOrphanedConstantsCount(constants)}", EditorStyles.miniLabel);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(constants);
        }
    }

    private void GroupConstants(GameConstants constants)
    {
        groupedConstants = new Dictionary<string, List<ConstantEntry>>();

        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return;

        foreach (var constant in constantsList)
        {
            var fieldInfo = typeof(ConstantType).GetField(constant.type.ToString());
            var attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;

            string category = "Other";

            if (attribute != null)
            {
                category = attribute.Category;
            }
            else if (!IsValidConstantType(constant.type))
            {
                category = "Orphaned (To Remove)";
            }

            if (!groupedConstants.ContainsKey(category))
                groupedConstants[category] = new List<ConstantEntry>();

            groupedConstants[category].Add(constant);
        }

        // Сортируем категории (Orphaned в конец)
        groupedConstants = groupedConstants
            .OrderBy(pair => pair.Key == "Orphaned (To Remove)" ? 1 : 0)
            .ThenBy(pair => pair.Key)
            .ToDictionary(pair => pair.Key, pair => pair.Value.OrderBy(c => c.type.ToString()).ToList());
    }

    private void AddMissingConstants(GameConstants constants)
    {
        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return;

        // Получаем все значения enum
        var allTypes = Enum.GetValues(typeof(ConstantType)).Cast<ConstantType>();
        var existingTypes = constantsList.Select(c => c.type).ToHashSet();

        int addedCount = 0;

        // Добавляем отсутствующие константы
        foreach (var type in allTypes)
        {
            if (!existingTypes.Contains(type))
            {
                var field = typeof(ConstantType).GetField(type.ToString());
                var attribute = Attribute.GetCustomAttribute(field, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;
                float defaultValue = attribute?.DefaultValue ?? 0f;

                constantsList.Add(new ConstantEntry { type = type, value = defaultValue });
                addedCount++;
                Debug.Log($"Added missing constant: {type} with value {defaultValue}");
            }
        }

        if (addedCount > 0)
        {
            EditorUtility.SetDirty(constants);
            Debug.Log($"Added {addedCount} missing constants");
        }
        else
        {
            Debug.Log("No missing constants found");
        }
    }

    private int RemoveOrphanedConstants(GameConstants constants)
    {
        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return 0;

        // Получаем все актуальные значения enum
        var validTypes = Enum.GetValues(typeof(ConstantType)).Cast<ConstantType>().ToHashSet();

        // Удаляем константы, которых нет в enum
        int removedCount = constantsList.RemoveAll(entry => !validTypes.Contains(entry.type));

        if (removedCount > 0)
        {
            EditorUtility.SetDirty(constants);
        }

        return removedCount;
    }

    private void ResetToDefaults(GameConstants constants)
    {
        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return;

        foreach (var constant in constantsList)
        {
            if (IsValidConstantType(constant.type))
            {
                constant.value = constant.GetDefaultValue();
            }
        }

        EditorUtility.SetDirty(constants);
    }

    private List<ConstantEntry> GetConstantsList(GameConstants constants)
    {
        var constantsField = typeof(GameConstants).GetField("constants",
            BindingFlags.NonPublic | BindingFlags.Instance);
        return constantsField?.GetValue(constants) as List<ConstantEntry>;
    }

    private bool IsValidConstantType(ConstantType type)
    {
        return Enum.IsDefined(typeof(ConstantType), type);
    }

    private int GetTotalConstantsCount(GameConstants constants)
    {
        var constantsList = GetConstantsList(constants);
        return constantsList?.Count ?? 0;
    }

    private int GetOrphanedConstantsCount(GameConstants constants)
    {
        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return 0;

        var validTypes = Enum.GetValues(typeof(ConstantType)).Cast<ConstantType>().ToHashSet();
        return constantsList.Count(entry => !validTypes.Contains(entry.type));
    }
}