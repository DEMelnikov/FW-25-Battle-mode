using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private CharacterSettings _settings;
    [SerializeField] private SO_CharacterStatsConfig _statsConfig;

    [SerializeField] private GameObject _selectedTarget;
    private CharacterStatsController _statsController;

    [SerializeField] public  SceneObjectTag SceneObjectTag {get; private set;}



    void Awake()
    {
        _statsController = GetComponent<CharacterStatsController>();
        if ( _statsController == null ) { Debug.Log("NO STATS CONTROLLER"); } else { Debug.Log("Stat controller is on"); }
        InitializeFromSettings();
    }

    #region Публичные свойства
    public GameObject GetSelectedTarget() => _selectedTarget;
    public void SetSelectedTarget(GameObject target) { _selectedTarget = target; }
    public CharacterStatsController GetStatsController() { return _statsController; }

    #endregion

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

        if (_statsConfig != null)
        {
            _statsController.SOIntializeStats(_statsConfig);
        }
        else
        {
            Debug.LogWarning("Character starting stats not assigned!", this);
            SceneObjectTag = SceneObjectTag.Hero;
            _selectedTarget = null;
        }
    }

}
