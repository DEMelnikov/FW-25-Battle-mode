using UnityEngine;

public class StateMachine : MonoBehaviour, IStateMachine
{
    [SerializeField] private MonoBehaviour _currentStateBehaviour;
    [SerializeField] private ScriptableObject _initialStateBehaviour;

                     private IState _currentState;
                     private IState _initialState;
                     private IStateContext _context;

    // ��� ������� � ����������
    [SerializeField] private string currentStateName;

    private void Start()
    {
        _context = new StateContext(gameObject);

        if (_initialStateBehaviour != null)
        {
            _initialState = _initialStateBehaviour as IState;
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
            // ���������� decisions �� ����������
            NotifyDecisionsUpdate(_currentState);


            // ���������������� ����� CheckTransitions ��� ��������� � ����������
            if (_currentState is StateWithTransitions stateWithTransitions)
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

    public void SetState(IState newState)
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


    public IState GetCurrentState()
    {
        return _currentState;
    }

    // ��������������� ������ ��� ������ � decisions
    private void NotifyDecisionsEnter(IState state)
    {
        if (state is StateWithTransitions stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnEnter(this);
            }
        }
    }
    private void NotifyDecisionsExit(IState state)
    {
        if (state is StateWithTransitions stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.GetTransitions())
            {
                transition.decision?.OnExit(this);
            }
        }
    }

    private void NotifyDecisionsUpdate(IState state)
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
    public void SetStateByType<T>() where T : IState
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