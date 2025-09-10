using UnityEngine;

public abstract class IAbility : ScriptableObject
{
    public abstract bool HasTag(string tag);
    public abstract bool CanAfford(IAbilityCostPayable user);
    public abstract bool PayAllCost(IAbilityCostPayable user);
}

