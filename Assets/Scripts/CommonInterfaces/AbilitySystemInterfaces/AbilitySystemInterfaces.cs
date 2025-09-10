using UnityEngine;

public abstract class IAbility : ScriptableObject
{
    public abstract bool HasTag(string tag);
    public abstract bool CanAfford(ICharacter character);
    public abstract bool PayAllCost(ICharacter character);
}

public interface IAbilityController
{

}

