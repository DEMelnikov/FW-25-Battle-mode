using UnityEngine;

public class StateMachine : MonoBehaviour, IStateMachine
{
    //[SerializeField] private ScriptableObject _currentStateBehaviour;
    //[SerializeField] private ScriptableObject _initialStateBehaviour;

    [SerializeReference] private State _currentState;
    [SerializeReference] private State _initialState;
                     private IStateContext _context;

    // Для отладки в инспекторе
    [SerializeField] private string currentStateName;

    private void Start()
    {
        _context = new StateContext(gameObject);

        if (_initialState != null)
        {
            //_initialState = _initialStateBehaviour as IState;
            SetState(_initialState);
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

    public void SetState(State newState)
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

    // Для перехода по типу состояния (удобно для скриптов)
    public void SetStateByType<T>() where T : State
    {
        // Здесь можно реализовать поиск состояния по типу
        // или использовать референсы заранее подготовленных состояний
    }

    // Доступ к контексту для состояний
    public IStateContext Context => _context;

    // Для установки внешних зависимостей
    //public void SetPlayerTarget(Transform playerTarget)
    //{
    //    _context.PlayerTarget = playerTarget;
    //}

}