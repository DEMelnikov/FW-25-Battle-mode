using UnityEngine;

public class StateMachine : MonoBehaviour, IStateMachine
{
    [SerializeField] private State _currentState;
    [SerializeField] private State _initialState;
                     private IStateContext _context;

    // ��� ������� � ����������
    [SerializeField] private string currentStateName;

    private void Start()
    {
        _context = new StateContext(gameObject);

        if (_initialState != null)
        {
            SetState(_initialState);
        }
    }

    private void Update()
    {
        if (PauseManager.IsPaused) return;
        if (_currentState != null)
        {
            // ���������� decisions �� ����������
            NotifyDecisionsUpdate(_currentState);

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

        // ���������� decisions �������� ��������� � ������
        NotifyDecisionsExit(_currentState);

        _currentState?.OnExit(this);
        _currentState = newState;
        currentStateName = newState?.name;

        // ���������� decisions ������ ��������� � �����
        NotifyDecisionsEnter(_currentState);

        _currentState?.OnEnter(this);
    }


    public State GetCurrentState()
    {
        return _currentState;
    }

    // ��������������� ������ ��� ������ � decisions
    private void NotifyDecisionsEnter(State state)
    {
        if (state is StateWithTransitions stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnEnter(this);
            }
        }
    }
    private void NotifyDecisionsExit(State state)
    {
        if (state is StateWithTransitions stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnExit(this);
            }
        }
    }

    private void NotifyDecisionsUpdate(State state)
    {
        if (state is StateWithTransitions stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnUpdate(this);
            }
        }
    }

    // ��� �������� �� ���� ��������� (������ ��� ��������)
    public void SetStateByType<T>() where T : State
    {
        // ����� ����� ����������� ����� ��������� �� ����
        // ��� ������������ ��������� ������� �������������� ���������
    }

    // ������ � ��������� ��� ���������
    public IStateContext Context => _context;

    // ��� ��������� ������� ������������
    //public void SetPlayerTarget(Transform playerTarget)
    //{
    //    _context.PlayerTarget = playerTarget;
    //}

}