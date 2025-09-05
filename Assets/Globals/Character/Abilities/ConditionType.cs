using UnityEngine;

[CreateAssetMenu(fileName = "NewCondition", menuName = "Abilities/Conditions/Base Condition")]
public class ConditionType : ScriptableObject
{
    [Header("Base Settings")]
    public string conditionName = "New Condition";
    [TextArea] public string description = "Condition description";
    public bool invertResult = false;

    [Header("UI Settings")]
    public Sprite icon;
    public Color displayColor = Color.white;

    public virtual bool CheckCondition(Character character)
    {
        // Базовая реализация - всегда true
        return !invertResult;
    }

    public virtual string GetStatusText(Character character)
    {
        bool result = CheckCondition(character);
        return $"{conditionName}: {(result ? "V" : "X")}";
    }

    public virtual float GetProgressValue(Character character)
    {
        return CheckCondition(character) ? 1f : 0f;
    }
}