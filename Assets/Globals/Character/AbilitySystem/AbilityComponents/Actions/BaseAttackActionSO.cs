using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "BaseAttackRollAction", menuName = "FW25/Ability System/Actions/Base Attack Roll")]
    public class BaseAttackRollAction : AbilityAction
    {
        [SerializeField] private StatTag _primaryAttackStat = StatTag.Agility;
        [SerializeField] private StatTag _secondaryAttackStat = StatTag.UnarmedAttack;
        [SerializeField] [Range(0, 100)]  private float _DC = 50f;
        //public string attackStat = "Attack";
        //public string defenseStat = "Defense";

        //public override ActionOutcome ExecuteAction(Character character)
        public override int ExecuteAction(Character character)
        {
            int outcome = 0;
            float rolls = 0;

            CharacterStatsController charStats = character.GetStatsController();
            rolls += charStats.Stats[_primaryAttackStat].Value;
            rolls += charStats.Stats[_secondaryAttackStat].Value;

            for (int i = 0; i <= rolls;)
            {
                if (Random.Range(0f, 100f) >= _DC) outcome++;
            }
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
            Debug.Log($"Character {character.name} make Attack roll & has {outcome} successes");
            return outcome;
        }
    }
}