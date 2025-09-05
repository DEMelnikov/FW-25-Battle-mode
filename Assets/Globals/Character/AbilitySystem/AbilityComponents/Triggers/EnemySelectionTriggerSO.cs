using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "EnemySelectionTrigger_Hero", menuName = "FW25/Ability System/Triggers/Enemy Selection(hero version)")]
    public class EnemySelectionTriggerSO : AbilityTrigger
    {
        //[Range(0f, 1f)] public float healthThreshold = 0.3f;
        //public bool belowThreshold = true;

        [SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Enemy;

        public override bool CheckTrigger(Character character)
        {
            if (character.GetSelectedTarget() == null) return false;
            GameObject _target = character.GetSelectedTarget();

            if (!character.GetSelectedTarget().TryGetComponent<Character>(out var targetCharacter))
                return false;

            if (targetCharacter.SceneObjectTag != _targetTag) return false; else return true;
                //var healthStat = character.Stats.GetStat("Health");
                //if (healthStat == null) return false;

                //float healthRatio = healthStat.CurrentValue / healthStat.MaxValue;
                //return belowThreshold ? healthRatio <= healthThreshold : healthRatio >= healthThreshold;

        }
    }
}