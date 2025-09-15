using AbilitySystem.AbilityComponents;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject
{
    [SerializeField] public bool logging = true;
    [SerializeField] private List<Trigger> abilityTriggers = new List<Trigger>();
    public virtual void OnEnter(IStateMachine machine) { }
    public virtual void OnExit(IStateMachine machine) { }
    public virtual void OnUpdate(IStateMachine machine) { }
    public virtual bool Decide(IStateMachine machine) { return false; }
}