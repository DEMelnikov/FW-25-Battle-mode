using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "EnemySelectionTrigger_Hero", menuName = "FW25/Ability System/Triggers/Enemy Selection(hero version)")]
    public class EnemySelectionTriggerSO : Trigger
    {
        //есть тесткейсы - EnemySelectionTriggerTests
        [SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Enemy;


        public override bool CheckTrigger(ICharacter character)
        {
            if (logging) Debug.Log("Check trigger EnemySelectionTriggerSO started");

            if (logging) Debug.Log("Check trigger EnemySelectionTriggerSO started");

            // Защита 1: Проверка на null character
            if (character == null)
            {
                if (logging) Debug.LogWarning("Character is null in EnemySelectionTriggerSO");
                return false;
            }

            // Защита 2: Проверка на наличие компонента Targets
            var targetsComponent = character.GetTargetsVault();
            if (targetsComponent == null)
            {
                if (logging) Debug.LogWarning("Targets component is null or missing");
                return false;
            }

            // BROKEN: Защита 3: Проверка на уничтоженный компонент Targets - 
            if (targetsComponent == null)// || !targetsComponent)
            {
                if (logging) Debug.LogWarning("Targets component is destroyed");
                return false;
            }

            GameObject target = targetsComponent.GetTargetEnemy();

            // Защита 3: Проверка на null цель
            if (target == null)
            {
                if (logging) Debug.Log($"Check trigger EnemySelectionTriggerSO: no selected target");
                return false;
            }

            // Защита 4: Корректная проверка уничтоженного Unity объекта
            if (target == null || !target) // Двойная проверка для Unity объектов
            {
                if (logging) Debug.LogWarning("Check trigger EnemySelectionTriggerSO: Selected target is destroyed");
                return false;
            }

            // Защита 5: Проверка на уничтоженный GameObject
            if (!target.activeInHierarchy) // Дополнительная проверка
            {
                if (logging) Debug.LogWarning("Check trigger EnemySelectionTriggerSO: Target is inactive or destroyed");
                return false;
            }

            // Защита 6: Безопасное получение компонента Character
            ICharacter targetCharacter = target.GetComponent<ICharacter>();
            if (targetCharacter == null)
            {
                if (logging) Debug.Log($"Check trigger EnemySelectionTriggerSO: no Character component at target object");
                return false;
            }

            // Защита 7: Проверка на уничтоженный компонент Character
            if (targetCharacter == null)// || !targetCharacter)  TODO - to fix
            {
                if (logging) Debug.LogWarning("Character component is destroyed");
                return false;
            }

            if (logging)
            {
                Debug.Log($"Check trigger EnemySelectionTriggerSO: get Target sceneObjectTag: {targetCharacter.SceneObjectTag} ");
                Debug.Log($"Check trigger EnemySelectionTriggerSO: get _targetTag: {_targetTag} ");
            }

            // Основная логика: проверка тега
            bool tagMatches = targetCharacter.SceneObjectTag == _targetTag;

            if (logging)
            {
                Debug.Log($"Check trigger EnemySelectionTriggerSO: " +
                         $"target tag = {targetCharacter.SceneObjectTag}, " +
                         $"required tag = {_targetTag}, " +
                         $"result = {(tagMatches ? "PASSED+++" : "FAILED---")}");
            }

            return tagMatches;
        }
    }
}