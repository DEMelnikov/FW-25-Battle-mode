using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;

[CustomEditor(typeof(GameConstants))]
public class GameConstantsEditor : Editor
{
    private Dictionary<string, List<ConstantEntry>> groupedConstants;
    private string exportPath = "Constants/constants.json";

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

                var oldValue = constant.value;
                constant.value = EditorGUILayout.FloatField(content, constant.value);

                if (Mathf.Abs(constant.value - (attribute?.DefaultValue ?? 0f)) > 0.01f)
                {
                    EditorGUILayout.LabelField($"Default: {attribute?.DefaultValue}", EditorStyles.miniLabel);
                }

                if (!IsValidConstantType(constant.type))
                {
                    EditorGUILayout.HelpBox($"Constant type '{constant.type}' no longer exists in enum!", MessageType.Warning);
                }
            }

            EditorGUI.indentLevel--;
            GUILayout.Space(10);
        }

        // Кнопки управления - ПЕРВАЯ ГРУППА
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

        // ВТОРАЯ ГРУППА КНОПОК
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

        // Секция Импорт/Экспорт
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Import / Export", EditorStyles.boldLabel);

        // ТРЕТЬЯ ГРУППА КНОПОК - Экспорт
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Export to JSON"))
        {
            ExportToJson(constants);
        }

        if (GUILayout.Button("Export to CSV"))
        {
            ExportToCsv(constants);
        }
        EditorGUILayout.EndHorizontal();

        // ЧЕТВЕРТАЯ ГРУППА КНОПОК - Импорт
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Import from JSON"))
        {
            string path = EditorUtility.OpenFilePanel("Import JSON", "", "json");
            if (!string.IsNullOrEmpty(path))
            {
                ImportFromJson(constants, path);
                GroupConstants(constants);
            }
        }

        if (GUILayout.Button("Import from CSV"))
        {
            string path = EditorUtility.OpenFilePanel("Import CSV", "", "csv");
            if (!string.IsNullOrEmpty(path))
            {
                ImportFromCsv(constants, path);
                GroupConstants(constants);
            }
        }
        EditorGUILayout.EndHorizontal();

        // Поле для быстрого экспорта
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Quick Export Path:", EditorStyles.miniLabel);
        exportPath = EditorGUILayout.TextField(exportPath);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Quick Export to JSON Path"))
        {
            QuickExportToJson(constants, exportPath);
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

    // МЕТОДЫ ЭКСПОРТА

    [System.Serializable]
    private class ExportData
    {
        public List<ExportConstant> constants = new List<ExportConstant>();
        public string exportDate;
        public string version = "1.0";
    }

    [System.Serializable]
    private class ExportConstant
    {
        public string type;
        public string name;
        public string category;
        public string description;
        public float value;
        public float defaultValue;

        public ExportConstant(ConstantEntry entry)
        {
            type = entry.type.ToString();
            value = entry.value;

            var fieldInfo = typeof(ConstantType).GetField(entry.type.ToString());
            var attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;

            name = attribute?.Category ?? "Unknown";
            category = attribute?.Category ?? "Other";
            description = attribute?.Description ?? "No description";
            defaultValue = attribute?.DefaultValue ?? 0f;
        }
    }

    private void ExportToJson(GameConstants constants)
    {
        string path = EditorUtility.SaveFilePanel("Export to JSON", "", "constants.json", "json");
        if (string.IsNullOrEmpty(path)) return;

        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return;

        var exportData = new ExportData
        {
            exportDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            constants = constantsList
                .Where(entry => IsValidConstantType(entry.type))
                .Select(entry => new ExportConstant(entry))
                .ToList()
        };

        string json = JsonUtility.ToJson(exportData, true);
        File.WriteAllText(path, json);

        Debug.Log($"Exported {exportData.constants.Count} constants to: {path}");
        EditorUtility.RevealInFinder(path);
    }

    private void QuickExportToJson(GameConstants constants, string relativePath)
    {
        string fullPath = Path.Combine(Application.dataPath, relativePath);
        string directory = Path.GetDirectoryName(fullPath);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return;

        var exportData = new ExportData
        {
            exportDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            constants = constantsList
                .Where(entry => IsValidConstantType(entry.type))
                .Select(entry => new ExportConstant(entry))
                .ToList()
        };

        string json = JsonUtility.ToJson(exportData, true);
        File.WriteAllText(fullPath, json);

        Debug.Log($"Quick exported {exportData.constants.Count} constants to: {fullPath}");
    }

    private void ExportToCsv(GameConstants constants)
    {
        string path = EditorUtility.SaveFilePanel("Export to CSV", "", "constants.csv", "csv");
        if (string.IsNullOrEmpty(path)) return;

        var constantsList = GetConstantsList(constants);
        if (constantsList == null) return;

        var csv = new StringBuilder();
        csv.AppendLine("Type,Category,Description,DefaultValue,CurrentValue");

        foreach (var entry in constantsList.Where(entry => IsValidConstantType(entry.type)))
        {
            var fieldInfo = typeof(ConstantType).GetField(entry.type.ToString());
            var attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;

            string category = attribute?.Category ?? "Other";
            string description = attribute?.Description ?? "No description";
            float defaultValue = attribute?.DefaultValue ?? 0f;

            // Экранируем запятые в описании
            description = description.Replace(",", ";");

            csv.AppendLine($"\"{entry.type}\",\"{category}\",\"{description}\",{defaultValue},{entry.value}");
        }

        File.WriteAllText(path, csv.ToString());

        Debug.Log($"Exported {constantsList.Count} constants to CSV: {path}");
        EditorUtility.RevealInFinder(path);
    }

    // МЕТОДЫ ИМПОРТА

    private void ImportFromJson(GameConstants constants, string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            var importData = JsonUtility.FromJson<ExportData>(json);

            var constantsList = GetConstantsList(constants);
            if (constantsList == null) return;

            int updatedCount = 0;
            int addedCount = 0;

            foreach (var importConstant in importData.constants)
            {
                if (Enum.TryParse<ConstantType>(importConstant.type, out var constantType))
                {
                    var existingEntry = constantsList.Find(entry => entry.type == constantType);

                    if (existingEntry != null)
                    {
                        existingEntry.value = importConstant.value;
                        updatedCount++;
                    }
                    else
                    {
                        constantsList.Add(new ConstantEntry
                        {
                            type = constantType,
                            value = importConstant.value
                        });
                        addedCount++;
                    }
                }
            }

            EditorUtility.SetDirty(constants);
            Debug.Log($"Imported from JSON: {updatedCount} updated, {addedCount} added");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import JSON: {e.Message}");
        }
    }

    private void ImportFromCsv(GameConstants constants, string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"File not found: {path}");
            return;
        }

        try
        {
            string[] lines = File.ReadAllLines(path);
            if (lines.Length < 2) return;

            var constantsList = GetConstantsList(constants);
            if (constantsList == null) return;

            int updatedCount = 0;

            for (int i = 1; i < lines.Length; i++) // Пропускаем заголовок
            {
                string[] fields = ParseCsvLine(lines[i]);
                if (fields.Length >= 5)
                {
                    string typeName = fields[0].Trim('"');
                    string valueStr = fields[4];

                    if (Enum.TryParse<ConstantType>(typeName, out var constantType) &&
                        float.TryParse(valueStr, out float value))
                    {
                        var existingEntry = constantsList.Find(entry => entry.type == constantType);
                        if (existingEntry != null)
                        {
                            existingEntry.value = value;
                            updatedCount++;
                        }
                    }
                }
            }

            EditorUtility.SetDirty(constants);
            Debug.Log($"Imported from CSV: {updatedCount} constants updated");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import CSV: {e.Message}");
        }
    }

    private string[] ParseCsvLine(string line)
    {
        var fields = new List<string>();
        bool inQuotes = false;
        int start = 0;

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (line[i] == ',' && !inQuotes)
            {
                fields.Add(line.Substring(start, i - start));
                start = i + 1;
            }
        }

        fields.Add(line.Substring(start));
        return fields.ToArray();
    }
}