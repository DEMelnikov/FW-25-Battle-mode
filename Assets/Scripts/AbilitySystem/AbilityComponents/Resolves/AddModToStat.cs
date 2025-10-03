
using UnityEngine;
using UnityEngine.TextCore.Text;


[CreateAssetMenu(fileName = "AddModToStat", menuName = "FW25/Ability System/Resolves/Add Modificator To Stat (by outcome)")]
public class AddModToStat : AbilityResolve
{
    [SerializeField] private StatTag _targetStat = StatTag.DefenceDC;
    [SerializeField] private ConstantType _constantToMultiply = ConstantType.SuccessDefenceCheckBonus;
    [SerializeField] private StatModType _modType = StatModType.Flat;
    [SerializeField] private bool isSubtraction = false;

    public override void ApplyResolve(ICharacter character, int outcome)
    {
        if (logging) Debug.Log($"Start Resolve {this.name}:");
        if (outcome <= 0)
        {
            if (logging) Debug.Log($"Resolve {this.name} outcome = 0 - no resolve");
            return;
        }
        var _target = GetTargetForAbility(character);

        if (_target == null) Debug.LogError("NO TARGET ENEMY");

        var stats = _target.StatController;
        float bonus = 0;

        bonus = Constants.Get(_constantToMultiply) * outcome;
        if (isSubtraction) bonus *= -1;

        StatModifier statModifier = new StatModifier(
            System.Guid.NewGuid().ToString(),
            _target.AbilityController.DefenceAbilityName,
            _targetStat,
            bonus,
            _modType );

        stats.Stats[_targetStat].AddModifier(statModifier);

        if (logging) Debug.Log($" Resolve {this.name} to {_target.ObjName}'s {_targetStat} added bonus {bonus} with tag {_target.AbilityController.DefenceAbilityName} new " +
            $"{_targetStat} = {stats.Stats[_targetStat].Value} ");

    }
}
