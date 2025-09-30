using UnityEngine;
using UnityEngine.AI;

public class StateWithPause : State
{
    protected bool _isSubscribed = false;
    protected Character owner;


    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        owner = owner = machine.Character;

        SubscribeToPauseEvents();

    }

    public override void OnExit(StateMachine machine)
    {
         // Отписываемся от события при выходе из состояния
        UnsubscribeFromPauseEvents();

        base.OnExit(machine);
    }

    public override void OnFixedUpdate(StateMachine machine)
    {
        if (PauseManager.IsPaused) return;
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(StateMachine machine)
    {
        if (PauseManager.IsPaused) return;
        base.OnUpdate(machine);

    }

    public void CheckTransitions(StateMachine machine)
    {
        base.CheckTransitions(machine);
    }

    // Метод для подписки на события паузы
    private void SubscribeToPauseEvents()
    {
        if (!_isSubscribed)
        {
            PauseManager.OnPauseStateChanged += HandlePauseStateChanged;
            _isSubscribed = true;
        }
    }

    // Метод для отписки от событий паузы
    private void UnsubscribeFromPauseEvents()
    {
        if (_isSubscribed)
        {
            PauseManager.OnPauseStateChanged -= HandlePauseStateChanged;
            _isSubscribed = false;
        }
    }

    // Обработчик изменения состояния паузы
    private void HandlePauseStateChanged(bool isPaused)
    {

    }
}
