using System.Collections.Generic;
using UnityEngine;

//version 1.2
public class StateMachine : MonoBehaviour, IStateMachine
{
    //[SerializeField] private ScriptableObject _currentStateBehaviour;
    //[SerializeField] private ScriptableObject _initialStateBehaviour;

    [SerializeReference] private State               _currentState;
    [SerializeReference] private State               _initialState;
    [SerializeReference] private State               _InEngageState;
                         private IStateContext       _context;
    [SerializeReference] private CharacterGlobalGoal _characterGoal = CharacterGlobalGoal.Idle;

    private Dictionary<string, State> stateInstances = new Dictionary<string, State>();


    // Для отладки в инспекторе
    [SerializeField] private string currentStateName;

    private void Start()
    {
        _context = new StateContext(gameObject);

        if (_initialState != null)
        {
            InitState(_InEngageState);
        }

        if (_initialState != null)
        {
            InitState(_initialState);
            SetStateById(_initialState.StateId);
        }


        //if (_initialState != null)
        //{
        //    SetState(_initialState);
        //}
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        if (_currentState != null)
        {
            // Уведомляем decisions об обновлении
            NotifyDecisionsUpdate(_currentState);


            // Централизованный вызов CheckTransitions для состояний с переходами
            if (_currentState is State stateWithTransitions)
            {
                stateWithTransitions.CheckTransitions(this);
            }
            
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

    public void OldSetState(State newState)
    {

        // Уведомляем decisions текущего состояния о выходе
        NotifyDecisionsExit(_currentState);

        _currentState?.OnExit(this);
        _currentState = newState;
        currentStateName = newState?.name;

        // Уведомляем decisions нового состояния о входе
        NotifyDecisionsEnter(_currentState);

        _currentState?.OnEnter(this);
    }

    public void SetState(State originalState)
    {
        InitState(originalState);
        SetStateById(originalState.StateId);
    }


    public State GetCurrentState()
    {
        return _currentState;
    }

    // Вспомогательные методы для работы с decisions
    private void NotifyDecisionsEnter(State state)
    {
        if (state is State stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnEnter(this);
            }
        }
    }
    private void NotifyDecisionsExit(State state)
    {
        if (state is State stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnExit(this);
            }
        }
    }

    private void NotifyDecisionsUpdate(State state)
    {
        if (state is State stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnUpdate(this);
            }
        }
    }

    private void InitState(State originalState)
    {
        if (!stateInstances.ContainsKey(originalState.StateId))
        {
            State clone = Instantiate(originalState);
            stateInstances[originalState.StateId] = clone;
        }
    }

    public void SetStateById(string stateId)
    {
        if (!stateInstances.ContainsKey(stateId))
        {
            Debug.LogError($"State with ID {stateId} not found in stateInstances.");
            return;
        }

        State newState = stateInstances[stateId];

        if (_currentState == newState) return;

        _currentState?.OnExit(this);
        _currentState = newState;
        currentStateName = newState.name;
        _currentState?.OnEnter(this);
    }

    public void SetStateInEngage()
    {
        SetStateById(_InEngageState.StateId);
    }

    // Доступ к контексту для состояний
    public IStateContext Context => _context;
    public CharacterGlobalGoal CharacterGoal { get => _characterGoal; set => _characterGoal = value; }

    public void SetInitialState()
    {
        SetStateById(_initialState.StateId);
    }
    // Для установки внешних зависимостей
    //public void SetPlayerTarget(Transform playerTarget)
    //{
    //    _context.PlayerTarget = playerTarget;
    //}

}