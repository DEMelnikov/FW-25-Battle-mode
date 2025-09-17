using System.Collections.Generic;
using UnityEngine;

public abstract class State : BaseState
{

    [SerializeField] public bool logging;

    [SerializeField] protected List<Transition> transitions = new List<Transition>();
    [SerializeField] protected State _allTransitiosFailedState;

    public virtual void OnEnter(IStateMachine machine) 
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Enter {this.name} State:");
    }
    public virtual void OnUpdate(IStateMachine machine) 
    {
        CheckTransitions(machine);
    }
    public virtual void OnFixedUpdate(IStateMachine machine) { }
    public virtual void OnExit(IStateMachine machine) { }

    public sealed override void CheckTransitions(IStateMachine machine)
    {
        if (logging) Debug.Log($"Checking transitions in state {this.name}");
        foreach (var transition in transitions)
        {
            if (logging) Debug.Log($" {this.name} Q transitions = {transitions.Count}");

            if (transition.decision != null && transition.trueState != null)
            {
                if (logging) Debug.Log($" {this.name}: Start = {transition.decision.name} result {transition.decision.Decide(machine)}");
                if (transition.decision.Decide(machine))
                {
                    machine.SetState(transition.trueState);
                    return;
                }
            }
        }
        if (logging) Debug.Log($"");

        if(_allTransitiosFailedState!= null) machine.SetState(_allTransitiosFailedState);
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}