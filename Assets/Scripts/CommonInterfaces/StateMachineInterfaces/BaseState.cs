using UnityEngine;

public class BaseState : ScriptableObject
{
    //[SerializeField] [TextArea(2,3)] protected string _description;
    //[SerializeField] public bool logging = true;
    //public virtual void OnEnter(IStateMachine machine) { }
    //public virtual void OnUpdate(IStateMachine machine) { }
    //public virtual void OnFixedUpdate(IStateMachine machine) { }
    //public virtual void OnExit(IStateMachine machine) { }

    //[SerializeField] public string Name { get; }    

    // Методы для проверки переходов
    public virtual void CheckTransitions(IStateMachine machine)
    {
        // Здесь будет логика проверки переходов между состояниями
    }
}