using System.Collections.Generic;
using UnityEngine;

public abstract class StateWithTransitions : State
{
    
    [SerializeField] private List<ITransition> transitions = new List<ITransition>();
    [SerializeField] private State _allTransitiosFailedState;

    public override void OnUpdate(IStateMachine machine)
    {
        CheckTransitions(machine);
    }

    protected override void CheckTransitions(IStateMachine machine)
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
                //else if (transition.falseState != null)
                //{
                //    machine.SetState(transition.falseState);
                //    return;
                //}
            }
        }

        if(_allTransitiosFailedState!= null) machine.SetState(_allTransitiosFailedState);
    }

    public List<ITransition> GetTransitions()
    {
        return transitions;
    }
}