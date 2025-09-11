using AbilitySystem;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour, ISelectableCharacter, ICharacter
{
    [Header("Настройки")]
    [SerializeField] private CharacterSettings         _settings;
    [SerializeField] private SO_CharacterStatsConfig   _statsConfig;

    [SerializeField] private GameObject                _selectedTarget;
    [SerializeField] public SceneObjectTag SceneObjectTag {get; private set;}

                     private IStateMachine              _stateMachine;
                     private CharacterStatsController  _statsController;
                     private IAbilityController         _abilityController;
                     private CharacterTargets          _targets;

    //public StateMaschine StateMaschine { get; set; }
    //public hState_Idle IdleState { get; set; }

    #region Unity methods
    void Awake()
    {
        _statsController = GetComponent<CharacterStatsController>();
        if ( _statsController == null ) { Debug.Log("NO STATS CONTROLLER"); } else { Debug.Log("Stat controller is on"); }
        InitializeFromSettings();
        _stateMachine = GetComponent<IStateMachine>();
        _targets = GetComponent<CharacterTargets>();

        //InitializeStateMachine();
    }

    void Update()
    {
        //StateMaschine.CurrentState.FrameUpdate();
    }

    void FixedUpdate()
    {
        //StateMaschine.CurrentState.PhysicUpdate();
    }
    #endregion

    #region Публичные свойства
    public GameObject GetSelectedTarget() => _selectedTarget; //TODO заменить
    public void SetSelectedTarget(GameObject target) { _selectedTarget = target; } //TODO заменить
    public IStatsController GetStatsController() { return _statsController; }
    public IStateMachine GetStateMachine() => _stateMachine;
    public IAbilityController GetAbilityController() => _abilityController;
    public ICharacterTargetsVault GetTargetsVault() => _targets;
    public Transform transform => this.transform;
    public string name => gameObject.name;
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
            //TODO - убрать заглушку
            //_selectedTarget = null;
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


    

    //private void InitializeStateMachine()
    //{
    //    StateMaschine = new StateMaschine();
        
    //    IdleState = new hState_Idle(this, StateMaschine);
    //    //AttackState = new AttackState(this, StateMachine);
    //    //MoveState = new MoveState(this, StateMachine);

    //    StateMaschine.Initialize(IdleState);
    //}

}
