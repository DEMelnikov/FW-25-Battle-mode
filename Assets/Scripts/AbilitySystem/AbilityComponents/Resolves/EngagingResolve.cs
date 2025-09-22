using UnityEngine;

[CreateAssetMenu(fileName = "EngagingResolve", menuName = "FW25/Ability System/Resolves/DefaultDamage")]
public class EngagingResolve : AbilityResolve
{
    public override void ApplyResolve(ICharacter character, int outcome)
    {
        var targetsVault = character.GetTargetsVault();
        if (!targetsVault.HasTargetEnemy()) return;
        

        if (targetsVault.TryGetTargetCharacter(out var targetCharacter))
        {
            if (!targetCharacter.InEngage)
            {
                targetCharacter.UnderMeleAttack();
            }
        }
    }
}
