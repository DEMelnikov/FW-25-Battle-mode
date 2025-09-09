using UnityEngine;
namespace AbilitySystem.AbilityComponents 
{
    [CreateAssetMenu(fileName = "No Selected enemy", menuName = "FW25/Ability System/Triggers/NPC/No Enemy Selection")]

    public class NoSelectedEnemy_NPCTrigger : AbilityTrigger
    {
        [SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Hero;

        public override bool CheckTrigger(Character character)
        {
            if (logging) Debug.Log($"{character.name} starts CheckTrigger NoSelectedEnemy");

            //var targetCharacter = character.GetComponent<Character>().GetSelectedTarget().GetComponent<Character>();

            if (character.GetTargets().HasTargetEnemy()) return true;

            character.GetTargets().TryGetTargetCharacter(out Character targetCharacter);
            if (targetCharacter == null) return false;

            bool tagMatches = targetCharacter.SceneObjectTag == _targetTag;

            if (logging)
            {
                Debug.Log($"Check trigger NoSelectedEnemy: " +
                         $"target tag = {targetCharacter.SceneObjectTag}, " +
                         $"required tag = {_targetTag}, " +
                         $"result = {(tagMatches ? "PASSED+++" : "FAILED---")}");
            }

            return tagMatches;
        }
    }
}


