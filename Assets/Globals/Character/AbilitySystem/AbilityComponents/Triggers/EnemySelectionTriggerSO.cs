using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "EnemySelectionTrigger_Hero", menuName = "FW25/Ability System/Triggers/Enemy Selection(hero version)")]
    public class EnemySelectionTriggerSO : AbilityTrigger
    {
        //[Range(0f, 1f)] public float healthThreshold = 0.3f;
        //public bool belowThreshold = true;

        [SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Enemy;
        //[SerializeField] private bool logging = true;

        public override bool CheckTrigger(Character character)
        {
            if (logging) Debug.Log("Check trigger EnemySelectionTriggerSO started");
            if (character == null)
            {
                if (logging) Debug.LogWarning("Character is null in EnemySelectionTriggerSO");
                return false;
            }

            GameObject target = character.GetSelectedTarget();
            if (target == null)
            {
                if (logging) Debug.Log($"Check trigger EnemySelectionTriggerSO: no selected target");
                return false;
            }

            // ������ 3: �������� ������������� �������
            if (target.Equals(null))
            {
                if (logging) Debug.LogWarning("Check trigger EnemySelectionTriggerSO: Selected target is destroyed");
                return false;
            }

            // ������ 4: ���������� ��������� ����������
            Character targetCharacter = target.GetComponent<Character>();
            if (targetCharacter == null)
            {
                if (logging) Debug.Log($"Check trigger EnemySelectionTriggerSO: no Character component at target object");
                return false;
            }

            // ������ 5: �������� �� ������������ ���������
            if (targetCharacter.Equals(null))
            {
                if (logging) Debug.LogWarning("Character component is destroyed");
                return false;
            }

            Debug.Log($"Check trigger EnemySelectionTriggerSO: get Target sceneObjectTag: {targetCharacter.SceneObjectTag} ");
            Debug.Log($"Check trigger EnemySelectionTriggerSO: get _targetTag: {_targetTag} ");

            // ������ 6: �������� ���� � null-check
            bool tagMatches = targetCharacter.SceneObjectTag == _targetTag;
            Debug.Log($"Check trigger EnemySelectionTriggerSO: get tagMatches: {tagMatches} ");

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