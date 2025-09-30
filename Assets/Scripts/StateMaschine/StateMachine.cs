using System.Collections.Generic;
using UnityEngine;

//version 2.0
public class StateMachine : MonoBehaviour, IStateMachine
{
    //[SerializeField] private ScriptableObject _currentStateBehaviour;
    //[SerializeField] private ScriptableObject _initialStateBehaviour;

    [SerializeReference] private State                _currentState;
    [SerializeField] 
                         //private IStateContext        _context;
                         private Character            _character;
    [SerializeReference] private CharacterGlobalState _characterGoal = CharacterGlobalState.Idle;
    [SerializeReference] private bool logging = false;


    [Header("Initial States")]
    [SerializeField] private State _idleState;
    [SerializeField] private State _chaseState;
    [SerializeField] private State _moveState;
    [SerializeField] private State _attackState;
    [SerializeField] private State _unconsciousState;
    [SerializeField] private State _inEngageState; 

    [Header("Actual States")]
    [SerializeField] private State idleState;
    [SerializeField] private State chaseState;
    [SerializeField] private State moveState;
    [SerializeField] private State attackState;
    [SerializeField] private State unconsciousState;
    [SerializeField] private State inEngageState;

    //private Dictionary<string, State> stateInstances = new Dictionary<string, State>();

    // Для отладки в инспекторе
    [SerializeField] private string currentStateName;

    #region Initialize
    public void Initialize()
    {
        //_сontext = new StateContext(gameObject);
        _character = GetComponent<Character>();

        IdleState = _idleState;
        ChaseState = _chaseState;
        MoveState = _moveState;
        AttackState = _attackState;
        UnconsciousState = _unconsciousState;
        InEngageState = _inEngageState;

        if (IdleState        == null || 
            ChaseState       == null || 
            MoveState        == null || 
            AttackState      == null ||
            InEngageState    == null ||
            UnconsciousState == null)
        {
            Debug.LogError($"{this.gameObject.name} - has no Initial State in SM");
            return;
        }

        this.SetState(FSMStates.IDLE);
    }

    public State IdleState
    {
        get => idleState;
        set
        {
            if (value != null && value.StateType == FSMStates.IDLE)
                //State clone = Instantiate(originalState);
            idleState = Instantiate(value);
        }
    }
    public State ChaseState
    {
        get => chaseState;
        set
        {
            if (value != null && value.StateType == FSMStates.CHASE)
                chaseState = Instantiate(value);
        }
    }
    public State MoveState
    {
        get => moveState;
        set
        {
            if (value != null && value.StateType == FSMStates.MOVE)
                moveState = Instantiate(value);
        }
    }
    public State AttackState
    {
        get => attackState;
        set
        {
            if (value != null && value.StateType == FSMStates.ATTACK)
                attackState = Instantiate(value);
        }
    }
    public State UnconsciousState
    {
        get => unconsciousState;
        set
        {
            if (value != null && value.StateType == FSMStates.UNCONSCIOUS)
                unconsciousState = Instantiate(value);
        }
    }

    public State InEngageState
    {
        get => inEngageState;
        set
        {
            if (value != null && value.StateType == FSMStates.INENGAGE)
                inEngageState = Instantiate(value);
        }
    }

    #endregion

    public State CurrentState => _currentState;
    public FSMStates CurrentStateFSM => _currentState.StateType;

    //public IStateContext Context { get => _context;}
    public CharacterGlobalState CharacterGoal { get => _characterGoal; set => _characterGoal = value; }
    public Character Character { get => _character; private set => _character = value; }
    //public ICharacter Character { get => _icharacter;  }


    public void SetState(FSMStates stateType)
    {
        switch (stateType)
        {
            case FSMStates.IDLE:
                ChangeState(idleState);
                break;
            case FSMStates.CHASE:
                ChangeState(chaseState);
                break;
            case FSMStates.MOVE:
                ChangeState(moveState);
                break;
            case FSMStates.ATTACK:
                ChangeState(attackState);
                break;
            case FSMStates.UNCONSCIOUS:
                ChangeState(unconsciousState);
                break;
            case FSMStates.INENGAGE:
                ChangeState(inEngageState); 
                break;
            default:
                Debug.LogError($"Unknown state {stateType}");
                break;
        }
    }
    private void ChangeState(State newState)
    {
        if (newState == null || newState == _currentState)
            return;

        _currentState?.OnExit(this);
        _currentState = newState;
        currentStateName = newState.name;
        _currentState?.OnEnter(this);
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        if (_currentState != null)
        {
            //// Уведомляем decisions об обновлении
            //NotifyDecisionsUpdate(_currentState);


            //// Централизованный вызов CheckTransitions для состояний с переходами
            //if (_currentState is State stateWithTransitions)
            //{
            //    stateWithTransitions.CheckTransitions(this);
            //}

            _currentState?.OnUpdate(this);
        }
    }

    private void FixedUpdate()
    {
        if (PauseManager.IsPaused) return;
        if (_currentState != null)
        {
            _currentState.OnFixedUpdate(this);
        }
    }

    //public void OldSetState(State newState)
    //{

    //    // Уведомляем decisions текущего состояния о выходе
    //    NotifyDecisionsExit(_currentState);

    //    _currentState?.OnExit(this);
    //    _currentState = newState;
    //    currentStateName = newState?.name;

    //    // Уведомляем decisions нового состояния о входе
    //    NotifyDecisionsEnter(_currentState);

    //    _currentState?.OnEnter(this);
    //}

    //public void SetState(State originalState)
    //{
    //    InitState(originalState);
    //    SetStateById(originalState.StateId);
    //}


    //public State GetCurrentState()
    //{
    //    return _currentState;
    //}

    //// Вспомогательные методы для работы с decisions
    //private void NotifyDecisionsEnter(State state)
    //{
    //    if (state is State stateWithTransitions)
    //    {
    //        foreach (var transition in stateWithTransitions.GetTransitions())
    //        {
    //            transition.decision?.OnEnter(this);
    //        }
    //    }
    //}
    //private void NotifyDecisionsExit(State state)
    //{
    //    if (state is State stateWithTransitions)
    //    {
    //        foreach (var transition in stateWithTransitions.GetTransitions())
    //        {
    //            transition.decision?.OnExit(this);
    //        }
    //    }
    //}

    //private void NotifyDecisionsUpdate(State state)
    //{
    //    if (state is State stateWithTransitions)
    //    {
    //        foreach (var transition in stateWithTransitions.GetTransitions())
    //        {
    //            transition.decision?.OnUpdate(this);
    //        }
    //    }
    //}

    //private void InitState(State originalState)
    //{
    //    if (!stateInstances.ContainsKey(originalState.StateId))
    //    {
    //        State clone = Instantiate(originalState);
    //        stateInstances[originalState.StateId] = clone;
    //    }
    //}

    //public void SetStateById(string stateId)
    //{
    //    if (!stateInstances.ContainsKey(stateId))
    //    {
    //        Debug.LogError($"State with ID {stateId} not found in stateInstances.");

    //        return;
    //    }

    //    State newState = stateInstances[stateId];

    //    if (_currentState == newState) return;

    //    _currentState?.OnExit(this);
    //    _currentState = newState;
    //    currentStateName = newState.name;
    //    _currentState?.OnEnter(this);
    //    if (logging) Debug.LogWarning($"{_context.Owner.name} - enter State {_currentState.StateId}");
    //}

    //public void SetStateInEngage()
    //{
    //    SetStateById(_InEngageState.StateId);
    //}

    //// Доступ к контексту для состояний
    //public IStateContext Context => _context;
    //public CharacterGlobalState CharacterGlobalState { get => _characterGoal; set => _characterGoal = value; }


    //public void SetInitialState()
    //{
    //    SetStateById(_initialState.StateId);
    //}

    public GameObject GetGameObject() => this.gameObject;
    // Для установки внешних зависимостей
    //public void SetPlayerTarget(Transform playerTarget)
    //{
    //    _context.PlayerTarget = playerTarget;
    //}

}