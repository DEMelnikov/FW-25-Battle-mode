using UnityEngine;


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
            GameObject enemy = character.GetTargetsVault().GetTargetEnemy();
            IStatsController enemyStats = character.GetTargetsVault().GetTargetEnemy().
                GetComponent<ICharacter>().GetStatsController();

            IStatsController heroStats = character.GetStatsController();

            enemyStats.Stats[StatTag.Health].AddToTmpModifier(outcome * _defaultDamage * -1);
            enemyStats.Stats[StatTag.Energy].AddToTmpModifier(outcome * _defaultDamage * heroStats.Stats[StatTag.Strength].Value*-1);
        }

    }
}
