using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Targets:MonoBehaviour
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
        if (_selectedTarget == null) return false;
        if (_selectedTarget.GetComponent<Character>() == null) 
        {
            _selectedTarget = null;
            return false;
        }

        if (_selectedTarget.GetComponent<Character>().SceneObjectTag != _whoIsYourEnemy) 
        {
            _selectedTarget = null;
            return false;
        }

        return true;
    }

    public GameObject GetTargetEnemy()
    {
        return _selectedTarget;
    }
}
