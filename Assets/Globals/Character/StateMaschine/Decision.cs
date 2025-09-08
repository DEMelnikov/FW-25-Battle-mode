using UnityEngine;

public abstract class Decision : ScriptableObject
{
    public virtual void OnEnter(StateMachine machine) { }
    public virtual void OnExit(StateMachine machine) { }
    public virtual void OnUpdate(StateMachine machine) { }
    public abstract bool Decide(StateMachine machine);
}