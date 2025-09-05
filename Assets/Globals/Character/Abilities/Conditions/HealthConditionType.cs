using UnityEngine;

[CreateAssetMenu(fileName = "HealthCondition", menuName = "Abilities/Condition Types/Health Condition")]
public class HealthConditionType : ConditionType
{
    public enum HealthCheckType
    {
        Percentage,
        Absolute,
        BelowValue,
        AboveValue
    }

    public HealthCheckType checkType = HealthCheckType.Percentage;

    public override bool CheckCondition(Character character)
    {
        if (character == null || character.Health == null) return false;

        bool result = false;
        float currentHealth = character.Health.Current;
        float maxHealth = character.Health.Max;

        switch (checkType)
        {
            case HealthCheckType.Percentage:
                result = (currentHealth / maxHealth) >= requiredValue;
                break;
            case HealthCheckType.Absolute:
                result = currentHealth >= requiredValue;
                break;
            case HealthCheckType.BelowValue:
                result = currentHealth <= requiredValue;
                break;
            case HealthCheckType.AboveValue:
                result = currentHealth >= requiredValue;
                break;
        }

        return invertResult ? !result : result;
    }

    public override string GetStatusText(Character character)
    {
        if (character == null) return "No character";

        float percentage = character.Health.Current / character.Health.Max;
        return $"{conditionName}: {percentage:P0} ({checkType} {requiredValue})";
    }
}