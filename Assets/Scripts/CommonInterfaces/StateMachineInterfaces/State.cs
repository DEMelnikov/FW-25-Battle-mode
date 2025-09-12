using System.Collections.Generic;
using UnityEngine;

public abstract class State : BaseState
{

    [SerializeField]
    [TextArea(2, 3)] protected string _description;
    [SerializeField] public bool logging;

    [SerializeField] protected List<Transition> transitions = new List<Transition>();
    [SerializeField] protected State _allTransitiosFailedState;

    public virtual void OnEnter(IStateMachine machine) { }
    public virtual void OnUpdate(IStateMachine machine) { }
    public virtual void OnFixedUpdate(IStateMachine machine) { }
    public virtual void OnExit(IStateMachine machine) { }

    public sealed override void CheckTransitions(IStateMachine machine)
    {
        foreach (var transition in transitions)
        {
            if (transition.decision != null && transition.trueState != null)
            {
                if (transition.decision.Decide(machine))
                {
                    machine.SetState(transition.trueState);
                    return;
                }
            }
        }

        if(_allTransitiosFailedState!= null) machine.SetState(_allTransitiosFailedState);
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}