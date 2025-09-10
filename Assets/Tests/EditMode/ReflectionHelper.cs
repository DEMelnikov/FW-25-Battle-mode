using System;
using System.Reflection;
using UnityEngine;

public static class ReflectionHelper
{
    // ������������� �������� private ���� ����� reflection
    public static void SetPrivateField(object obj, string fieldName, object value)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        // ���� ���� � �������� ������������ (FlattenHierarchy)
        FieldInfo field = obj.GetType().GetField(fieldName,
            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        if (field == null)
        {
            Debug.LogError($"Field '{fieldName}' not found in type '{obj.GetType().Name}'");
            return;
        }

        field.SetValue(obj, value);
    }

    // �������� �������� private ���� ����� reflection
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