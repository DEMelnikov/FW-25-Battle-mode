using UnityEngine;
using UnityEngine.AI;

public class StateWithPause : State
{
    protected bool _isSubscribed = false;
    protected Character owner;


    public override void OnEnter(IStateMachine machine)
    {
        base.OnEnter(machine);
        var owner = machine.Context.Owner.GetComponent<Character>();

        SubscribeToPauseEvents();

    }

    public override void OnExit(IStateMachine machine)
    {
         // ������������ �� ������� ��� ������ �� ���������
        UnsubscribeFromPauseEvents();

        base.OnExit(machine);
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        if (PauseManager.IsPaused) return;
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {
        if (PauseManager.IsPaused) return;
        base.OnUpdate(machine);

    }

    public void CheckTransitions(IStateMachine machine)
    {
        base.CheckTransitions(machine);
    }

    // ����� ��� �������� �� ������� �����
    private void SubscribeToPauseEvents()
    {
        if (!_isSubscribed)
        {
            PauseManager.OnPauseStateChanged += HandlePauseStateChanged;
            _isSubscribed = true;
        }
    }

    // ����� ��� ������� �� ������� �����
    private void UnsubscribeFromPauseEvents()
    {
        if (_isSubscribed)
        {
            PauseManager.OnPauseStateChanged -= HandlePauseStateChanged;
            _isSubscribed = false;
        }
    }

    // ���������� ��������� ��������� �����
    private void HandlePauseStateChanged(bool isPaused)
    {

    }
}
