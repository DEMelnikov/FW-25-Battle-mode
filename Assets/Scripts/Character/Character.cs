using Unity.Android.Gradle;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.AI;


public class Character : MonoBehaviour, ISelectableCharacter, ICharacter
{
    [Header("Настройки")]
    [SerializeField] private CharacterSettings       _settings;
    [SerializeField] private SO_CharacterStatsConfig _statsConfig;
    [SerializeField] private bool                    logging = false;

    //[SerializeField] private GameObject                _selectedTarget;
    [SerializeField] public SceneObjectTag SceneObjectTag { get; private set; }

    private IStateMachine               _stateMachine;
    private CharacterStatsController    _statsController;
    private IAbilityController          _abilityController;
    private CharacterTargetsVault       _targets;
    private NavMeshAgent                _navMeshAgent;
    private IBehaviorProfile            _behaviorProfile;
    [SerializeField] private bool       _inEngage;

    [SerializeField] private Animator   animator;

    //public StateMaschine StateMaschine { get; set; }
    //public hState_Idle IdleState { get; set; }

    #region Unity methods
    void Awake()
    {
        StatController = GetComponent<CharacterStatsController>();
        if (_statsController == null) { Debug.Log("NO STATS CONTROLLER"); } else { if (logging) Debug.Log("Stat controller is on"); }

        InitializeFromSettings();

        _abilityController = GetComponent<IAbilityController>();

        animator = GetComponentInChildren<Animator>();//GetComponentIn<Animator>();
        if (animator == null) Debug.LogError("No Animator");

        _stateMachine = GetComponent<IStateMachine>();


        _targets = GetComponent<CharacterTargetsVault>();
        _behaviorProfile = GetComponent<BehaviorProfile>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        if (_navMeshAgent == null) { Debug.Log("NO _navMeshAgent CONTROLLER"); } else { if (logging) Debug.Log("_navMeshAgent is on"); }
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;


        _abilityController.Initialize();
        _stateMachine.Initialize();
        _targets.Initialize();



        //agent.SetDestination(new Vector3(0, 0, transform.position.z));

        //InitializeStateMachine();
        //Transform child = transform.Find("SPUM_20240911215638389");
        //if (child != null)
        //{
        //    animator = child.GetComponent<Animator>();
        //}
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

    //[System.Obsolete("Use CharacterTargetVaults")]
    //public GameObject GetSelectedTarget()
    //{
    //    if (_targets.TryGetTargetEnemy(out GameObject enemyGameobject)) return enemyGameobject;
    //    return null;
    //}

    //TODO заменить и возмоно убрать
    //public void SetSelectedTarget(GameObject target)
    //{
    //    _targets.SetTargetEnemy(target);
    //}
    //[System.Obsolete("Use StatContoller")]
    public IStatsController StatController { get => _statsController; private set => _statsController = value as CharacterStatsController; }
    //public IStatsController StatController 
    public IStateMachine StateMachine { get => _stateMachine; } //private set => _stateMachine = value as Stat; }
    public IAbilityController AbilityController { get => _abilityController; }
    public Animator Animator { get=> animator;  }
    public ICharacterTargetsVault TargetsVault { get => _targets; private set => _targets = value as CharacterTargetsVault; }
    public Transform transform => this.gameObject.transform;
    public GameObject GetGameObject => this.gameObject;
    public string ObjName => gameObject.name;
    public bool InEngage { get => _inEngage; set => _inEngage = value; }
    public NavMeshAgent GetNavMeshAgent() => _navMeshAgent;
    public IBehaviorProfile BehaviorProfile { get => _behaviorProfile; } 
    public void UnderMeleAttack(ICharacter agressor)
    {
        _inEngage = true;
        if (!_targets.HasTargetEnemy())
        {
            _targets.SetTargetEnemyCharacter(agressor);
        }
        _stateMachine.SetState(FSMStates.INENGAGE);
    }
    #endregion

    #region Animations
    public void ActivateAnimationTrigger(AnimationTriggers trigger)
    {
        Debug.Log($"Using Animation Trigger {trigger.ToString()}");
        if(trigger!=AnimationTriggers.EMPTY) animator.SetTrigger(trigger.ToString());
    }
    public void SetAnimationBool(AnimationBools stateName, bool _bool)
    {

        //Debug.LogError($"Using Animation BOOOL {stateName.ToString()} {_bool}");
        animator.SetBool(stateName.ToString(), _bool);
        //animator.
        //animator.SetBool("ISDEAD", true);
        //owner.SetAnimationBool(AnimationBools.ISDEAD.ToString(), true);

        //animator.SetTrigger("TOIDLE");
    }

    public void AnimatorPlayIdle()
    {
        //Debug.LogWarning("TRYING PLAY IDLE!");
        animator.Play("IDLE");
    }
    
    #endregion

    public void InitializeFromSettings()
    {
        if (_settings != null)
        {
            SceneObjectTag = _settings.SceneObjectTag;
            //_targets.SetTargetEnemy(null);
            //if (_settings.DefaultTarget == null) _targets.SetTargetEnemy(null); else _targets.SetTargetEnemy(_settings.DefaultTarget);
            //_targets.SetTargetEnemy(_settings.DefaultTarget); 
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
            //_selectedTarget = null;
        }
    }



}
