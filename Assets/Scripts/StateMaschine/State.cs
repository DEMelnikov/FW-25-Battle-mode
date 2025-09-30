using System.Collections.Generic;
using UnityEngine;

public abstract class State : BaseState
{

    [SerializeField] public bool logging;

    [SerializeField] protected List<Transition> transitions = new List<Transition>();
    //[SerializeField] protected FSMStates _allTransitiosFailedState;

    public virtual void OnEnter(IStateMachine machine) 
    {
        if (logging) Debug.LogWarning($"{machine.GetGameObject().name} Enter {this.name} State:");
    }
    public virtual void OnUpdate(IStateMachine machine) 
    {
        CheckTransitions(machine);
    }
    public virtual void OnFixedUpdate(IStateMachine machine) { }
    public virtual void OnExit(IStateMachine machine) 
    {
        if (logging) Debug.Log($"{machine.GetGameObject().name} Exit {this.name} State");
    }

    public sealed override void CheckTransitions(IStateMachine machine)
    {
        //if (logging) Debug.Log($"Checking transitions in state {this.name}");
        if (logging) Debug.Log($" {machine.GetGameObject().name} State {this.name} Start Checking transitions Q = {transitions.Count}");
        foreach (var transition in transitions)
        {
            if (transition.decision != null && transition.trueState != null)
            {
                if (transition.decision.Decide(machine.Character))
                {
                    if (logging) Debug.Log($" {this.name}: Decision: {transition.decision.name} result TRUE! Next State {transition.trueState.ToString()}");
                    //if (logging) Debug.Log($" {this.name}: Decision: {transition.decision.name} result true ");

                    if (transition.IsInfluenceCharacterGlobalGoal) machine.CharacterGoal = transition.GetNewGlobalGoal;
                    //if (transition.SetIdleState)
                    //{
                    //    machine.CharacterGoal = CharacterGlobalState.Idle;
                    //    machine.SetState(FSMStates.IDLE);
                    //    return;
                    //}
                    machine.SetState(transition.trueState);
                    return;
                }
            }
        }
        //if (logging) Debug.Log($" {this.name}: All Transitions failed ");

        
        //if (_allTransitiosFailedState != null) machine.SetState(transition.);
    }

    public List<Transition> GetTransitions()
    {
        return transitions;
    }
}