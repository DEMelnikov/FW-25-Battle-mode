using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SumStatsRoll", menuName = "FW25/Ability System/Actions/SumStatsRoll")]
public class SumStatsRoll : AbilityAction
{
    //[SerializeField] private StatTag _primaryAttackStat = StatTag.Agility;
    //[SerializeField] private StatTag _secondaryAttackStat = StatTag.UnarmedAttack;

    [SerializeField] protected List<StatTag> stats = new List<StatTag>();
    [SerializeField] protected ConstantType defaultDC;


    //[SerializeField] [Range(0, 100)]  private float _DC = 50f;
    //public string attackStat = "Attack";
    //public string defenseStat = "Defense";

    //public override ActionOutcome ExecuteAction(Character character)
    public override int ExecuteAction(ICharacter character)
    {
        //float defaultDC = Constants.attackDC;
        return Action(character, Constants.attackDC);
    }

    public override int ExecuteAction(ICharacter character, float myDC)
    {
        return Action(character, myDC);
    }


    private int CalculateRolls(ICharacter character)
    {
        float countRolls = 0; 
        
        if (stats.Count == 0)
        {
            Debug.LogError($"{character.GetGameObject.name} - empty Stats collection in action {this.name}");
            return 0;
        }

        IStatsController charStats = character.GetStatsController();
        if (charStats == null)
        {
            Debug.LogError($"BaseAttackRollAction: can't get CharacterStatsController "); 
            return 0;
        }

        foreach ( var item in stats)
        {
            countRolls += charStats.Stats[item].Value;
            if (logging) Debug.Log($"{this.name}: Got Stat: {item.ToString()} value = {charStats.Stats[item].Value} sumrolls: {countRolls}");
        }

        return  Mathf.RoundToInt(countRolls);
    }

    private int Action(ICharacter character, float DC)
    {

        if (logging) { Debug.Log($"Character {character.ObjName} starts action BaseAttackRollAction "); }

        int outcome = 0;
        int rolls = 0;

        IStatsController charStats = character.GetStatsController();
        if (charStats == null)
        {
            if (logging) { Debug.Log($"BaseAttackRollAction: can't get CharacterStatsController "); }
            return 0;
        }

        rolls = CalculateRolls(character);

        for (int i = 0; i < rolls; i++)
        {
            var roll = Random.Range(0f, 100f);
            if (logging) { Debug.Log($"{this.name}: Roll {i} result  = {roll} "); }
            if (roll >= DC) outcome++;
        }

        if (logging) { Debug.Log($"Character {character.ObjName} make rolls vs DC {DC} & has {outcome} successes"); }
        return outcome;
    }

}
