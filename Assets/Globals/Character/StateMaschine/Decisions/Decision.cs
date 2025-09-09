using AbilitySystem.AbilityComponents;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject
{
    [SerializeField] public bool logging = true;
    [SerializeField] private List<AbilityTrigger> abilityTriggers = new List<AbilityTrigger>();
    public virtual void OnEnter(StateMachine machine) { }
    public virtual void OnExit(StateMachine machine) { }
    public virtual void OnUpdate(StateMachine machine) { }
    public abstract bool Decide(StateMachine machine);
}