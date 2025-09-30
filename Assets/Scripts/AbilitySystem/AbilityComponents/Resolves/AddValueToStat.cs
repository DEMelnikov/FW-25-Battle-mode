using UnityEngine;


[CreateAssetMenu(fileName = "AddValueToStat", menuName = "FW25/Ability System/Resolves/Add Value To Stat")]
public class AddValueToStat : AbilityResolve
{
    [SerializeField][Min(0.1f)] private float _addValue = 1f;
    [SerializeField]private StatTag _statName = StatTag.ENERGY;

    public override void ApplyResolve(ICharacter character, int outcome)
    {

        if (outcome > 0)
        {
            //GameObject enemy = character.GetTargetsVault().GetTargetEnemy();
            var enemyStats = character.GetStatsController();

            enemyStats.Stats[_statName].AddToTmpModifier(outcome * _addValue);
            //enemyStats.Stats[StatTag.ENERGY].AddToTmpModifier(outcome * _defaultDamage * heroStats.Stats[StatTag.Strength].Value * -1);
        }
    }
}
