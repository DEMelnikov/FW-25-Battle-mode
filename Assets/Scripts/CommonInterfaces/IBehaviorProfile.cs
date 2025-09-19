using UnityEngine;

public interface IBehaviorProfile
{
    float PursuitDistance { get; }
    float WeaponRange { get; }
    IAbility BaseAttackAbility { get; }
    float BaseAttackInterval {  get; }
}
