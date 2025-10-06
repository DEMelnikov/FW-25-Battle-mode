using UnityEngine;

public interface IBehaviorProfile
{
    float PursuitDistance { get; }
    float WeaponRange { get; }
    float BaseAttackInterval {  get; }
    float WalkEnergyCost { get; }
    CoupDeGrace CoupDeGraceSettings { get; set; }
}
