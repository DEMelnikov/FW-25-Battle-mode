using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class ConstantEntry
{
    public ConstantType type;
    public float value;

    // ДОБАВЛЕН метод GetDefaultValue
    public float GetDefaultValue()
    {
        var field = typeof(ConstantType).GetField(type.ToString());
        var attribute = Attribute.GetCustomAttribute(field, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;
        return attribute?.DefaultValue ?? 0f;
    }
}

[CreateAssetMenu(fileName = "GameConstants", menuName = "Game/Constants Container")]
public class GameConstants : ScriptableObject
{
    [SerializeField] private List<ConstantEntry> constants = new List<ConstantEntry>();
    private Dictionary<ConstantType, float> constantsDictionary;

    public void Initialize()
    {
        constantsDictionary = constants.ToDictionary(x => x.type, x => x.value);
    }

    public float GetValue(ConstantType type)
    {
        if (constantsDictionary == null)
            Initialize();

        if (constantsDictionary.TryGetValue(type, out float value))
            return value;

        Debug.LogError($"Constant of type {type} not found!");
        return GetDefaultValue(type); // Используем новый метод
    }

    // ДОБАВЛЕН метод для получения значения по умолчанию
    private float GetDefaultValue(ConstantType type)
    {
        var field = typeof(ConstantType).GetField(type.ToString());
        var attribute = Attribute.GetCustomAttribute(field, typeof(ConstantInfoAttribute)) as ConstantInfoAttribute;
        return attribute?.DefaultValue ?? 0f;
    }
}