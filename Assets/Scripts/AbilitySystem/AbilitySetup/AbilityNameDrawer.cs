#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(AbilityNameAttribute))]
public class AbilityNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Забираем все Ability из Vault
        var vault = AbilitiesVault.Instance;
        string[] names;

        if (vault != null && vault.abilities != null)
        {
            names = vault.abilities.Select(a => a.GetAbilityName()).ToArray();
        }
        else
        {
            names = new string[0];
        }

        // Определяем текущий индекс выбранного элемента
        int currentIndex = Mathf.Max(0, System.Array.IndexOf(names, property.stringValue));

        // Отрисовываем выпадающий список
        int newIndex = EditorGUI.Popup(position, label.text, currentIndex, names);

        if (newIndex >= 0 && newIndex < names.Length)
        {
            property.stringValue = names[newIndex];
        }
    }
}
#endif
