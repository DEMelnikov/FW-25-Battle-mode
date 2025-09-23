using UnityEngine;
using UnityEngine.AI;


public class Character : MonoBehaviour, ISelectableCharacter, ICharacter
{
    [Header("Настройки")]
    [SerializeField] private CharacterSettings         _settings;
    [SerializeField] private SO_CharacterStatsConfig   _statsConfig;

    [SerializeField] private GameObject                _selectedTarget;
    [SerializeField] public SceneObjectTag SceneObjectTag {get; private set;}

                     private IStateMachine             _stateMachine;
                     private CharacterStatsController  _statsController;
                     private IAbilityController        _abilityController;
                     private CharacterTargets          _targets;
                     private NavMeshAgent              _navMeshAgent;
                     private IBehaviorProfile          _behaviorProfile;
    [SerializeField] private bool                      _inEngage;

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
        _behaviorProfile = GetComponent<BehaviorProfile>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis   = false;
        //agent.SetDestination(new Vector3(0, 0, transform.position.z));

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
    public void SetSelectedTarget(GameObject target) 
    {
        _targets.SetTargetEnemy(target);
    } 
    //TODO заменить
    public IStatsController GetStatsController() { return _statsController; }
    public IStateMachine GetStateMachine() => _stateMachine;
    public IAbilityController GetAbilityController() => _abilityController;
    public ICharacterTargetsVault GetTargetsVault() => _targets;
    public Transform transform => this.gameObject.transform;
    public GameObject GetGameObject => this.gameObject;
    public string name => gameObject.name;
    public bool InEngage { get => _inEngage; set => _inEngage = value; }

    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;
    public IBehaviorProfile GetBehaviorProfile() => _behaviorProfile;
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

    public void UnderMeleAttack(GameObject agressor)
    {
        _inEngage = true;
        if (!_targets.HasTargetEnemy())
        {
            _targets.SetTargetEnemy(agressor);
        }
        _stateMachine.SetStateInEngage();
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
