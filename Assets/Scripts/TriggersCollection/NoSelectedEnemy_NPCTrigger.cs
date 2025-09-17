using UnityEngine;
namespace AbilitySystem.AbilityComponents 
{
    [CreateAssetMenu(fileName = "No Selected enemy", menuName = "FW25/Triggers/NPC/No Enemy Selection")]

    public class NoSelectedEnemy_NPCTrigger : Trigger
    {
        //[SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Hero;

        public override bool CheckTrigger(ICharacter character)
        {
            if (logging) Debug.Log($"{character.name} starts CheckTrigger NoSelectedEnemy");

            // Используем новый безопасный метод
            if (character.GetTargetsVault().TryGetTargetEnemy(out _))
                return true;

            character.GetTargetsVault().TryGetTargetCharacter(out ICharacter targetCharacter);
            if (targetCharacter == null)
            {
                if (logging) Debug.Log($"{character.name} Targetenemy is empty check pass");
                return true;
            }
            
            if (logging) Debug.Log($"{character.name} Targetenemy is not empty check not pass");

            return false;

            //bool tagMatches = targetCharacter.SceneObjectTag == _targetTag;

            //if (logging)
            //{
            //    Debug.Log($"Check trigger NoSelectedEnemy: " +
            //             $"target tag = {targetCharacter.SceneObjectTag}, " +
            //             $"required tag = {_targetTag}, " +
            //             $"result = {(tagMatches ? "PASSED+++" : "FAILED---")}");
            //}

            //return tagMatches;
        }
    }
}


