using UnityEngine;

public abstract class State : ScriptableObject
{
    public virtual void OnEnter(StateMachine machine) { }
    public virtual void OnUpdate(StateMachine machine) { }
    public virtual void OnFixedUpdate(StateMachine machine) { }
    public virtual void OnExit(StateMachine machine) { }

    // Методы для проверки переходов
    protected virtual void CheckTransitions(StateMachine machine)
    {
        // Здесь будет логика проверки переходов между состояниями
    }
}