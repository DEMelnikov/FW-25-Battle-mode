using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SumOfStatsAsMultiplerToConst", menuName = "FW25/Ability System/Ability Cost/Sum of Stats as multiplier to constant")]
public class SumOfStatsAsMultiplerToConst : AbilityCost
{
    [SerializeField] private bool logging = true;
    [SerializeField] private StatTag _statToAddModifier =  StatTag.ENERGY;
    [SerializeField] private List<StatTag> _listStatsToSum = new();
    [SerializeField] private ConstantType _constant = ConstantType.DefenceEnergyCost;
    [SerializeField] private bool isSubtraction = true;

    public override bool PayAbilityCost(ICharacter character)
    {
        if (logging) Debug.Log($"{character.ObjName} -  {this.name} : paying cost");

        if (CanAffordCost(character))
        {
            float result = GetResult(character);
            character.            StatController.Stats[_statToAddModifier].AddToTmpModifier(result);

            if (logging) Debug.Log($"{character.ObjName} - {this.name} : paying cost {result} from {_statToAddModifier} stat");
            return true;
        }

        if (logging) Debug.Log($"{character.ObjName} - {this.name}  finished unsuccess");
        return false;
    }

    public override bool CanAffordCost(ICharacter character)
    {
        IStatsController heroStats = character.StatController;
        if (heroStats.Stats[StatTag.ENERGY].Value + GetResult(character) > 0 ) return true; else return false;
    }

    private float GetResult(ICharacter character)
    {
        float multiplier = 0;
        IStatsController heroStats = character.StatController;

        foreach (StatTag stat in _listStatsToSum)
        {
            multiplier += heroStats.Stats[stat].Value;
        }

        if (isSubtraction) multiplier *= -1;

        return multiplier * Constants.Get(_constant);
    }
}

