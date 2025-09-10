using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "DamageResolve", menuName = "FW25/Ability System/Resolves/DefaultDamage")]
    public class DamageResolveSO : AbilityResolve
    {
        [SerializeField][Min(0.1f)] private float _defaultDamage = 1f;
        //public float baseDamage = 10f;
        //public string damageStat = "Strength";

        public override void ApplyResolve(ICharacter character, int outcome)
        {

            if (outcome > 0)
            {
                GameObject enemy = character.GetTargets().GetTargetEnemy();
                CharacterStatsController enemyStats = character.GetTargets().GetTargetEnemy().
                    GetComponent<Character>().GetStatsController();

                CharacterStatsController heroStats = character.GetStatsController();

                enemyStats.Stats[StatTag.Health].AddToTmpModifier(outcome * _defaultDamage * -1);
                enemyStats.Stats[StatTag.Energy].AddToTmpModifier(outcome * _defaultDamage * heroStats.Stats[StatTag.Strength].Value*-1);
            }


            //if (outcome.result == ActionResult.Success || outcome.result == ActionResult.CriticalSuccess)
            //{
            //    float statMultiplier = character.Stats.GetStat(damageStat)?.CurrentValue ?? 1f;
            //    float damage = baseDamage * statMultiplier * outcome.effectiveness;

            //    // «десь должна быть логика применени€ урона цели
            //    Debug.Log($"Applied {damage} damage with effectiveness {outcome.effectiveness}");
            //}
        }
    }
}