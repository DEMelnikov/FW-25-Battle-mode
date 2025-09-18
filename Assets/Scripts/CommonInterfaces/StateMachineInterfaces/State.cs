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
    public virtual void OnExit(IStateMachine machine) 
    {
        if (logging) Debug.Log($"{machine.Context.Owner.name} Exit {this.name} State");
    }

    public sealed override void CheckTransitions(IStateMachine machine)
    {
        //if (logging) Debug.Log($"Checking transitions in state {this.name}");
        if (logging) Debug.Log($" State {this.name} Start Checking transitions Q = {transitions.Count}");
        foreach (var transition in transitions)
        {
            if (transition.decision != null && transition.trueState != null)
            {
                if (transition.decision.Decide(machine))
                {
                    if (logging) Debug.Log($" {this.name}: Decision: {transition.decision.name} result TRUE! Next State {transition.trueState.name}");
                    //if (logging) Debug.Log($" {this.name}: Decision: {transition.decision.name} result true ");
                    machine.SetState(transition.trueState);
                    return;
                }
            }
        }
        //if (logging) Debug.Log($" {this.name}: All Transitions failed ");

        if (_allTransitiosFailedState!= null) machine.SetState(_allTransitiosFailedState);
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}