using UnityEngine;
using UnityEngine.TextCore.Text;


[CreateAssetMenu(fileName = "AddValueToStat", menuName = "FW25/Ability System/Resolves/Add Value To Stat (multiplied outcome)")]
public class AddValueToStat : AbilityResolve
{
    [SerializeField] private float   _addValue = 1f;
    [SerializeField] private StatTag _statName = StatTag.ENERGY;

    [Header("Aditional multiplier:")]
    [SerializeField] private bool useAditionalMultiplier = false;
    [SerializeField] private StatTag _statNameAditional = StatTag.ENERGY;
    [SerializeField] protected AbilityTargets _addStatOwner = AbilityTargets.SELF;


    public override void ApplyResolve(ICharacter character, int outcome)
    {
        if (logging) Debug.Log($"Start Resolve {this.name}:");
        if (outcome <= 0) return;
        
        var _target = GetTargetForAbility(character);
        if (_target == null) Debug.LogError("NO TARGET ENEMY");

        var enemyStats = _target.GetStatsController();
        float multiplier = 1f;

        //Debug.LogWarning($"Enemy Stats is TryGetComponent targetICharacter name  ");

        if (logging) Debug.Log($"Resolve {this.name} check for multiplier:");
        if (useAditionalMultiplier)  multiplier = GetAditionalMultiplier(character); 

        enemyStats.Stats[_statName].AddToTmpModifier(outcome * _addValue * multiplier);

        if (logging) Debug.Log($" Resolve {this.name} to {_target.GetGameObject.name}'s {_statName} added {outcome * _addValue * multiplier} with multiplier {multiplier}");         //enemyStats.Stats[StatTag.ENERGY].AddToTmpModifier(outcome * _defaultDamage * heroStats.Stats[StatTag.Strength].Value * -1);
        
    }

    private float GetAditionalMultiplier(ICharacter character)
    {
        float multiplier;

        var _target = GetTargetForAbility(character, _addStatOwner);
        var enemyStats = _target.GetStatsController();

        multiplier = enemyStats.Stats[_statNameAditional].Value;

        if (multiplier == 0 ) multiplier = 1f;

        if (logging) Debug.Log($" Resolve {this.name} to {_target.GetGameObject.name}'s has aditional multiplier {_statName} = {multiplier}");

        return multiplier;
    }
}
