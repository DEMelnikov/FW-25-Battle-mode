using UnityEngine;

public class CharacterTargets : MonoBehaviour, ICharacterTargetsVault
{
    [SerializeField] private GameObject _selectedTarget;
    [SerializeField] private Transform _waypoint;
    [SerializeField] private SceneObjectTag _whoIsYourEnemy = SceneObjectTag.Enemy;
    [SerializeField] private bool logging = true;

    // ����� ����� - ���������� ��������� ��������� ����
    public bool TryGetTargetEnemy(out GameObject targetEnemy)
    {
        if (_selectedTarget != null &&
            _selectedTarget.GetComponent<Character>() != null &&
            _selectedTarget.GetComponent<Character>().SceneObjectTag == _whoIsYourEnemy)
        {
            targetEnemy = _selectedTarget;
            if (logging) Debug.Log($"{gameObject.name} CharacterTargets.TryGetTargetEnemy() " +
                $"- valid enemy target: {targetEnemy.name}");
            return true;
        }

        targetEnemy = null;
        if (logging) Debug.Log($"{gameObject.name} CharacterTargets.TryGetTargetEnemy() " +
            $"- no valid enemy target");
        return false;
    }

    // ����������� HasTargetEnemy - ������ ��� �������� ��������!
    public bool HasTargetEnemy()
    {
        return TryGetTargetEnemy(out _); // ���������� ����� �����
    }

    // ��������� � ������� - ��������� �����
    public void ValidateAndCleanTarget()
    {
        if (!TryGetTargetEnemy(out _)) // ���� ���� ���������
        {
            if (logging && _selectedTarget != null)
            {
                Debug.Log($"{gameObject.name} CharacterTargets.ValidateAndCleanTarget() " +
                    $"- cleaning invalid target: {_selectedTarget.name}");
            }
            _selectedTarget = null;
        }
    }

    public void SetTargetEnemy(GameObject target)
    {
        if (target == null) return;

        var targetCharacter = target.GetComponent<Character>();
        if (targetCharacter == null) return;

        if (targetCharacter.SceneObjectTag == _whoIsYourEnemy)
        {
            _selectedTarget = target;
            if (logging) Debug.Log($"{gameObject.name} Get new Alive Target {_selectedTarget.name}");
        }
        else if (logging)
        {
            Debug.Log($"{gameObject.name} SetTargetEnemy rejected " +
                $"- target tag {targetCharacter.SceneObjectTag} != enemy tag {_whoIsYourEnemy}");
        }
    }

    
    public GameObject GetTargetEnemy()
    {
        return TryGetTargetEnemy(out var target) ? target : null;
    }

    public bool TryGetTargetCharacter(out ICharacter targetCharacter)
    {
        if (_selectedTarget != null && _selectedTarget.TryGetComponent<ICharacter>(out targetCharacter))
            return true;

        targetCharacter = null;
        return false;
    }
}