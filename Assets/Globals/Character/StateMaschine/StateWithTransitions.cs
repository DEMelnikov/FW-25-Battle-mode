using System.Collections.Generic;
using UnityEngine;

public abstract class StateWithTransitions : State
{
    [System.Serializable]
    public class Transition
    {
        public Decision decision;
        public State trueState;
        public State falseState;
    }

    [SerializeField] private List<Transition> transitions = new List<Transition>();

    public override void OnUpdate(StateMachine machine)
    {
        CheckTransitions(machine);
    }

    protected override void CheckTransitions(StateMachine machine)
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
                else if (transition.falseState != null)
                {
                    machine.SetState(transition.falseState);
                    return;
                }
            }
        }
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}