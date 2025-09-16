#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SONameDropdownAttribute))]
public class SONameDropdownListDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Высота на каждый элемент + заголовок
        return EditorGUIUtility.singleLineHeight * (property.arraySize + 1) + 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = attribute as SONameDropdownAttribute;
        if (attr == null || attr.VaultType == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        // Отрисовать заголовок сверху
        Rect listLabelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(listLabelRect, label);

        // Найти Vault ассет
        var guids = AssetDatabase.FindAssets($"t:{attr.VaultType.Name}");
        if (guids == null || guids.Length == 0)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        ScriptableObject vaultSO = AssetDatabase.LoadAssetAtPath(assetPath, attr.VaultType) as ScriptableObject;
        if (vaultSO == null)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        var itemsField = attr.VaultType.GetField("items", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        List<string> names = new List<string>();
        if (itemsField != null)
        {
            if (itemsField.GetValue(vaultSO) is IEnumerable items)
            {
                foreach (var item in items)
                {
                    if (item is UnityEngine.Object obj)
                        names.Add(obj.name);
                }
            }
        }

        if (names.Count == 0)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        // Начинаем отрисовку элементов (списка строк) ниже заголовка
        Rect elementRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);

        EditorGUI.indentLevel++;

        for (int i = 0; i < property.arraySize; i++)
        {
            SerializedProperty element = property.GetArrayElementAtIndex(i);

            int currentIndex = Mathf.Max(0, names.IndexOf(element.stringValue));
            int newIndex = EditorGUI.Popup(elementRect, $"Trigger {i}", currentIndex, names.ToArray());

            if (newIndex != currentIndex)
            {
                element.stringValue = names[newIndex];
            }

            elementRect.y += EditorGUIUtility.singleLineHeight + 2;
        }
        EditorGUI.indentLevel--;
    }
}
#endif
