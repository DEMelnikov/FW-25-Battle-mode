#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SONameDropdownAttribute))]
public class SONameDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as SONameDropdownAttribute;
        if (attr == null || attr.VaultType == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        // Поиск Vault-ассета
        var guids = AssetDatabase.FindAssets($"t:{attr.VaultType.Name}");
        if (guids == null || guids.Length == 0)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        ScriptableObject vaultSO = AssetDatabase.LoadAssetAtPath(assetPath, attr.VaultType) as ScriptableObject;

        if (vaultSO == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        var itemsField = attr.VaultType.GetField("items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

        if (itemsField == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        List<string> names = new List<string>();

        if (itemsField.GetValue(vaultSO) is IEnumerable items)
        {
            foreach (var item in items)
            {
                var unityObj = item as UnityEngine.Object;
                if (unityObj != null)
                    names.Add(unityObj.name);
            }
        }

        if (names.Count == 0)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        int currentIndex = Mathf.Max(0, names.IndexOf(property.stringValue));

        // Отобразить попап
        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, names.ToArray());

        if (selectedIndex >= 0 && selectedIndex < names.Count)
        {
            property.stringValue = names[selectedIndex];
        }
        else
        {
            property.stringValue = "";
        }
    }
}
#endif
