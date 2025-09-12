using UnityEngine;

public class BaseState : ScriptableObject
{
    public virtual void CheckTransitions(IStateMachine machine)
    {
        // Здесь будет логика проверки переходов между состояниями
    }
}