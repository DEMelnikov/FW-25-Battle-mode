using UnityEngine;

[DisallowMultipleComponent]
public class CharacterTargets : MonoBehaviour, ICharacterTargetsVault
{
    [SerializeField] private GameObject _selectedTarget;
    [SerializeField] private Transform _waypoint;
    [SerializeField] private SceneObjectTag _whoIsYourEnemy = SceneObjectTag.Enemy;
    [SerializeField] protected bool logging = true;

    // Новый метод - безопасное получение вражеской цели
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

    public bool TryGetTargetEnemyTransform (out Transform _transform)
    {
        if (TryGetTargetEnemy(out GameObject _enemy))
        {
            if (logging) Debug.Log($"{gameObject.name} CharacterTargets.TryGetTargetEnemy() Got {_enemy.name} and now geting Transform");
            _transform = _enemy.transform;
            if (logging) Debug.Log($" Transform position X: {_transform.position.x}");
            return true; 
        }

        _transform = null;
        return false;
    }

    // Обновленный HasTargetEnemy - теперь без побочных эффектов!
    public bool HasTargetEnemy()
    {
        if (logging) Debug.Log($"{gameObject.name} CharacterTargets.TryGetTargetEnemy() " +
    $"- no valid enemy target");
        return TryGetTargetEnemy(out _); // Используем новый метод
    }

    // Валидация и очистка - отдельный метод
    public void ValidateAndCleanTarget()
    {
        if (!TryGetTargetEnemy(out _)) // Если цель невалидна
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
        //Debug.Break();

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

        //Debug.Break();
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