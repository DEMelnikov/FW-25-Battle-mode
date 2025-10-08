using UnityEngine;

[CreateAssetMenu(fileName = "Attack-SumStatsRoll", menuName = "FW25/Ability System/Actions/Attack-SumStatsRoll")]
public class SumStatsRoll_AttackMode : SumStatsRoll
{
    [Header("target Stat to Check")]
    [SerializeField] protected StatTag targetDC = StatTag.DefenceDC;

    public override int ExecuteAction(ICharacter character)
    {
        if (!character.TargetsVault.TryGetTargetCharacter(out ICharacter targetCharacter))
            Debug.LogError($"{character.ObjName}.{this.name} - can't find target character to initiate defence");

        targetCharacter.AbilityController.TryExecuteDefenceAbility();

        float DC = targetCharacter.StatController.GetStat(targetDC).Value;

        targetCharacter.StatController.GetStat(targetDC).RemoveModifiersFromSource(targetCharacter.AbilityController.DefenceAbilityName);

        //Debug.LogError($"{character.ObjName}.{this.name} - modifier removed - value is {targetCharacter.StatController.Stats[targetDC].Value}");

        return Action(character, DC);
    }
}
