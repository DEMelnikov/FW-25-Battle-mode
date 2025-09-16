using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "EnemySelectionTrigger_Hero", menuName = "FW25/Ability System/Triggers/Enemy Selection(hero version)")]
    public class EnemySelectionTriggerSO : Trigger
    {
        //���� ��������� - EnemySelectionTriggerTests
        [SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Enemy;


        public override bool CheckTrigger(ICharacter character)
        {
            if (logging) Debug.Log("Check trigger EnemySelectionTriggerSO started");

            if (logging) Debug.Log("Check trigger EnemySelectionTriggerSO started");

            // ������ 1: �������� �� null character
            if (character == null)
            {
                if (logging) Debug.LogWarning("Character is null in EnemySelectionTriggerSO");
                return false;
            }

            // ������ 2: �������� �� ������� ���������� Targets
            var targetsComponent = character.GetTargetsVault();
            if (targetsComponent == null)
            {
                if (logging) Debug.LogWarning("Targets component is null or missing");
                return false;
            }

            // BROKEN: ������ 3: �������� �� ������������ ��������� Targets - 
            if (targetsComponent == null)// || !targetsComponent)
            {
                if (logging) Debug.LogWarning("Targets component is destroyed");
                return false;
            }

            GameObject target = targetsComponent.GetTargetEnemy();

            // ������ 3: �������� �� null ����
            if (target == null)
            {
                if (logging) Debug.Log($"Check trigger EnemySelectionTriggerSO: no selected target");
                return false;
            }

            // ������ 4: ���������� �������� ������������� Unity �������
            if (target == null || !target) // ������� �������� ��� Unity ��������
            {
                if (logging) Debug.LogWarning("Check trigger EnemySelectionTriggerSO: Selected target is destroyed");
                return false;
            }

            // ������ 5: �������� �� ������������ GameObject
            if (!target.activeInHierarchy) // �������������� ��������
            {
                if (logging) Debug.LogWarning("Check trigger EnemySelectionTriggerSO: Target is inactive or destroyed");
                return false;
            }

            // ������ 6: ���������� ��������� ���������� Character
            ICharacter targetCharacter = target.GetComponent<ICharacter>();
            if (targetCharacter == null)
            {
                if (logging) Debug.Log($"Check trigger EnemySelectionTriggerSO: no Character component at target object");
                return false;
            }

            // ������ 7: �������� �� ������������ ��������� Character
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

            // �������� ������: �������� ����
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