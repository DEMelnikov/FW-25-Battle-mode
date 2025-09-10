using System;
using System.Reflection;
using UnityEngine;

public static class ReflectionHelper
{
    // Устанавливает значение private поля через reflection
    public static void SetPrivateField(object obj, string fieldName, object value)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        // Ищем поле в иерархии наследования (FlattenHierarchy)
        FieldInfo field = obj.GetType().GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        if (field == null)
        {
            Debug.LogError($"Field '{fieldName}' not found in type '{obj.GetType().Name}'");
            return;
        }

        field.SetValue(obj, value);
    }

    // Получает значение private поля через reflection
    public static T GetPrivateField<T>(object obj, string fieldName)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        FieldInfo field = obj.GetType().GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        if (field == null)
        {
            Debug.LogError($"Field '{fieldName}' not found in type '{obj.GetType().Name}'");
            return default;
        }

        return (T)field.GetValue(obj);
    }
}