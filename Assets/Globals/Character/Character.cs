using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private CharacterSettings _settings;
    [SerializeField] private GameObject _selectedTarget;

    public  SceneObjectTag SceneObjectTag {get; private set;}



    void Awake()
    {
        InitializeFromSettings();
    }

    public GameObject GetSelectedTarget() => _selectedTarget;
    public void SetSelectedTarget(GameObject target) { _selectedTarget = target; }

    public void InitializeFromSettings()
    {
        if (_settings != null)
        {
            SceneObjectTag = _settings.SceneObjectTag;
            _selectedTarget = _settings.DefaultTarget;
        }
        else
        {
            Debug.LogWarning("Character settings not assigned!", this);
            SceneObjectTag = SceneObjectTag.Hero;
            _selectedTarget = null;
        }
    }

}
