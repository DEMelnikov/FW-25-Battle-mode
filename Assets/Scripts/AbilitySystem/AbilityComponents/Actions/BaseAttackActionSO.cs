using UnityEngine;


[CreateAssetMenu(fileName = "BaseAttackRollAction", menuName = "FW25/Ability System/Actions/Base Attack Roll")]
public class BaseAttackRollAction : AbilityAction
{
    [SerializeField] private StatTag _primaryAttackStat = StatTag.Agility;
    [SerializeField] private StatTag _secondaryAttackStat = StatTag.UnarmedAttack;
    [SerializeField] [Range(0, 100)]  private float _DC = 50f;
    //public string attackStat = "Attack";
    //public string defenseStat = "Defense";

    //public override ActionOutcome ExecuteAction(Character character)
    public override int ExecuteAction(ICharacter character)
    {
        if (logging) { Debug.Log($"Character {character.name} starts action BaseAttackRollAction "); }

        int outcome = 0;
        float rolls = 0;

        IStatsController charStats = character.GetStatsController();
        if (charStats == null) 
        {
            if (logging) { Debug.Log($"BaseAttackRollAction: can't get CharacterStatsController "); }
            return 0; 
        }



        rolls += charStats.Stats[_primaryAttackStat].Value;
        rolls += charStats.Stats[_secondaryAttackStat].Value;

        if (logging) { Debug.Log($"BaseAttackRollAction: Got _primaryAttackStat   rolls = {rolls} "); }
        if (logging) { Debug.Log($"BaseAttackRollAction: Got _secondaryAttackStat rolls = {rolls} "); }

        for (int i = 0; i <= rolls; i++)
        {
            var roll = Random.Range(0f, 100f);
            if (logging) { Debug.Log($"BaseAttackRollAction: Roll {i} result  = {roll} "); }
            if ( roll >= _DC) outcome++;
        }

        //if (logging) { Debug.Log($"BaseAttackRollAction: Made {rolls} and got  = {outcome} "); }

        //var outcome = new ActionOutcome();
        //float attackValue = character.Stats.GetStat(attackStat)?.CurrentValue ?? 0;
        //float defenseValue = character.Stats.GetStat(defenseStat)?.CurrentValue ?? 0;

        //float roll = Random.Range(0f, 1f);
        //float successChance = Mathf.Clamp(attackValue / (attackValue + defenseValue), 0.1f, 0.9f);

        //if (roll <= successChance * 0.2f)
        //{
        //    outcome.result = ActionResult.CriticalSuccess;
        //    outcome.effectiveness = 2.0f;
        //    outcome.message = "Critical hit!";
        //}
        //else if (roll <= successChance)
        //{
        //    outcome.result = ActionResult.Success;
        //    outcome.effectiveness = 1.0f;
        //    outcome.message = "Attack successful!";
        //}
        //else if (roll >= 0.95f)
        //{
        //    outcome.result = ActionResult.CriticalFailure;
        //    outcome.effectiveness = 0.5f;
        //    outcome.message = "Critical failure!";
        //}
        //else
        //{
        //    outcome.result = ActionResult.Failure;
        //    outcome.effectiveness = 0.8f;
        //    outcome.message = "Attack failed!";
        //}
        if (logging) { Debug.Log($"Character {character.name} make Attack roll & has {outcome} successes"); }
        return outcome;
    }
}
