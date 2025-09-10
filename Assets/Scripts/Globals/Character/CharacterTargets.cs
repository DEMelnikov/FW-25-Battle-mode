using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CharacterTargets:MonoBehaviour
{
    [SerializeField] private GameObject     _selectedTarget;
    [SerializeField] private Transform      _waypoint;
    [SerializeField] private SceneObjectTag _whoIsYourEnemy = SceneObjectTag.Enemy;
    [SerializeField] private bool           logging = true;
    public void SetTargetEnemy(GameObject target)
    {
        if (target == null) return;
        if (target.GetComponent<Character>() == null) return;
        if (target.GetComponent<Character>().SceneObjectTag != _whoIsYourEnemy) return;
        _selectedTarget = target;
        if (logging) Debug.Log($"{gameObject.name} Get new Alive Target {_selectedTarget.name}");
    }

    public bool HasTargetEnemy()
    {
        if (logging) Debug.Log($"{gameObject.name} CharacterTargets.HasTargetEnemy() check start");
        if (_selectedTarget == null) 
        {
            if (logging) Debug.Log($"{gameObject.name} CharacterTargets.HasTargetEnemy() _selectedTarget==null check over");
            return false;
        }

        if (logging) Debug.Log($"{gameObject.name} CharacterTargets.HasTargetEnemy() _selectedTarget!=null");
        if (_selectedTarget.GetComponent<Character>() == null) 
        {
            _selectedTarget = null;
            return false;
        }
        if (logging) Debug.Log($"{gameObject.name} CharacterTargets.HasTargetEnemy() _selectedTarget has CharacterComponent");
        if (_selectedTarget.GetComponent<Character>().SceneObjectTag != _whoIsYourEnemy) 
        {
            if (logging) Debug.Log($"{gameObject.name} CharacterTargets.HasTargetEnemy() _selectedTarget " +
                $"has tag{_selectedTarget.GetComponent<Character>().SceneObjectTag} " +
                $"_whoIsYourEnemy = {_whoIsYourEnemy} Check FAILED---");

            _selectedTarget = null;
            return false;
        }
        if (logging) Debug.Log($"{gameObject.name} CharacterTargets.HasTargetEnemy() check Success");

        return true;
    }

    public GameObject GetTargetEnemy()
    {
        return _selectedTarget;
    }

    public bool TryGetTargetCharacter(out Character targetCharacter)
    {
        if (_selectedTarget != null && _selectedTarget.TryGetComponent<Character>(out targetCharacter)) return true;
        targetCharacter = null;
        return false;
    }
}
