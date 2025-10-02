using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(GameConstants))]
public class GameConstantsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var constants = (GameConstants)target;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Game Constants", EditorStyles.boldLabel);
        GUILayout.Space(5);

        // Простой список без группировки для начала
        EditorGUILayout.LabelField("Constants List:", EditorStyles.boldLabel);

        // Получаем поле constants через сериализацию
        var serializedObject = new SerializedObject(constants);
        var constantsProperty = serializedObject.FindProperty("constants");

        EditorGUILayout.PropertyField(constantsProperty, true);

        serializedObject.ApplyModifiedProperties();

        GUILayout.Space(10);

        if (GUILayout.Button("Initialize Constants"))
        {
            constants.Initialize();
            EditorUtility.SetDirty(constants);
        }

        // Кнопка для добавления всех отсутствующих констант
        if (GUILayout.Button("Add Missing Constants"))
        {
            AddMissingConstants(constants);
        }
    }

    private void AddMissingConstants(GameConstants constants)
    {
        // Получаем все значения enum
        var allTypes = Enum.GetValues(typeof(ConstantType));

        // Получаем поле constants через рефлексию
        var constantsField = typeof(GameConstants).GetField("constants",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var constantsList = constantsField?.GetValue(constants) as List<ConstantEntry>;

        if (constantsList == null) return;

        // Добавляем отсутствующие константы
        foreach (ConstantType type in allTypes)
        {
            if (!constantsList.Exists(c => c.type == type))
            {
                var field = typeof(ConstantType).GetField(type.ToString());
                var attribute = Attribute.GetCustomAttribute(field, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;
                float defaultValue = attribute?.DefaultValue ?? 0f;

                constantsList.Add(new ConstantEntry { type = type, value = defaultValue });
                Debug.Log($"Added missing constant: {type} with value {defaultValue}");
            }
        }

        EditorUtility.SetDirty(constants);
    }
}