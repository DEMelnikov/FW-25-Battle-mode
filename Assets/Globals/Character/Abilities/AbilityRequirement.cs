using UnityEngine;

[CreateAssetMenu(fileName = "AbilityRequirement", menuName = "Abilities/Requirements/Base Requirement")]
public class AbilityRequirement : ScriptableObject
{
    public string requirementName = "New Requirement";
    [TextArea] public string description;

    public virtual bool CheckRequirement(Character character)
    {
        return true;
    }

    public virtual string GetStatusText(Character character)
    {
        return $"{requirementName}: {(CheckRequirement(character) ? "V" : "X")}";
    }
}