using UnityEngine;


[CreateAssetMenu(fileName = "AddValueToStat", menuName = "FW25/Ability System/Resolves/Add Value To Stat (multiplied outcome)")]
public class AddValueToStat : AbilityResolve
{
    [SerializeField] private float   _addValue = 1f;
    [SerializeField] private StatTag _statName = StatTag.ENERGY;

    public override void ApplyResolve(ICharacter character, int outcome)
    {
        if (outcome > 0)
        {
            var _target = GetTargetForAbility(character);
            var enemyStats = _target.GetStatsController();

            enemyStats.Stats[_statName].AddToTmpModifier(outcome * _addValue);
            //enemyStats.Stats[StatTag.ENERGY].AddToTmpModifier(outcome * _defaultDamage * heroStats.Stats[StatTag.Strength].Value * -1);
        }
    }
}
