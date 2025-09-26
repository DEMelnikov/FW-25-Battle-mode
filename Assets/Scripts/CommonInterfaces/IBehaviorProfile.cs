using UnityEngine;

public interface IBehaviorProfile
{
    float PursuitDistance { get; }
    float WeaponRange { get; }
    Ability BaseAttackAbility { get; }
    float BaseAttackInterval {  get; }
    float WalkEnergyCost { get; }
}
