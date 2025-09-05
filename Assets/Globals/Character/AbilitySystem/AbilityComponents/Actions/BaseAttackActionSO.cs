using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "BaseAttackRollAction", menuName = "FW25/Ability System/Actions/Base Attack Roll")]
    public class BaseAttackRollAction : AbilityAction
    {
        //public string attackStat = "Attack";
        //public string defenseStat = "Defense";

        public override ActionOutcome ExecuteAction(Character character)
        {



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

            return outcome;
        }
    }
}