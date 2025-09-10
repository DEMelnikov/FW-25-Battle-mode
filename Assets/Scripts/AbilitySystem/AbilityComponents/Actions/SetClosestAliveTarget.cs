using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;
using Transform = UnityEngine.Transform;
namespace AbilitySystem.AbilityComponents 
{
    [CreateAssetMenu(fileName = "SetClosetAliveTargetAction", menuName = "FW25/Ability System/Actions/Set Closest Alive Target")]
    public class SetClosestAliveTarget : AbilityAction
    {
        [SerializeField][Min(0.1f)] private float maxDistance = 200f;
        [SerializeField]            private SceneObjectTag _targetTag = SceneObjectTag.Hero;

        private Transform myPosition;


        public override int ExecuteAction(ICharacter character)
        {
            if (logging) Debug.Log($"{character.name} starts Action SetTarget and try find Enemy with tag {_targetTag}");

            myPosition = character.transform;

            // Находим все объекты с компонентом Character
            Character[] allCharacters = FindObjectsByType<Character>(FindObjectsSortMode.None);
            List<Character> validTargets = new List<Character>();

            if (logging) Debug.Log($"{character.name} Action SetTarget find {allCharacters.Length} Characters");

            // Собираем все подходящие цели
            foreach (Character ch in allCharacters)
            {
                float distance = Vector3.Distance(myPosition.position, ch.transform.position);

                if (ch.SceneObjectTag == _targetTag && distance <= maxDistance)
                {
                    validTargets.Add(ch);
                }
            }

            if (logging) Debug.Log($"{character.name} Action SetTarget find {validTargets.Count} Enemies with tag {_targetTag}");


            // Если нет подходящих целей
            if (validTargets.Count == 0)
            {
                return 0;
            }

            // Ищем ближайшую цель
            Character closestTarget = FindClosestTarget(validTargets);

            if (closestTarget != null)
            {
                GameObject closestGO = closestTarget.gameObject;
                character.SetSelectedTarget(closestGO);
                if (logging) Debug.Log($"{character.name} Action SetTarget find closest target with name {closestGO.name}");

                return 1;
            }

            return 0;

        }

        private Character FindClosestTarget(List<Character> targets)
        {
            Character closest = null;
            float minDistance = Mathf.Infinity;

            foreach (Character target in targets)
            {
                float distance = Vector3.Distance(myPosition.position, target.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = target;
                }
            }

            return closest;
        }
    }
}


