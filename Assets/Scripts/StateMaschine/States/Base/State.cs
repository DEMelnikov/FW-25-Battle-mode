using System.Collections.Generic;
using UnityEngine;

public abstract class State : BaseState
{

    [SerializeField] public bool logging;

    [SerializeField] protected List<Transition> transitions = new List<Transition>();

    protected Character owner;
    //[SerializeField] protected FSMStates _allTransitiosFailedState;

    public virtual void OnEnter(StateMachine machine) 
    {
        if (logging) Debug.LogWarning($"{machine.GetGameObject().name} Enter {this.name} State:");
        owner = machine.Character;
    }
    public virtual void OnUpdate(StateMachine machine) 
    {
        CheckTransitions(machine);
    }
    public virtual void OnFixedUpdate(StateMachine machine) { }
    public virtual void OnExit(StateMachine machine) 
    {
        if (logging) Debug.Log($"{machine.GetGameObject().name} Exit {this.name} State");
    }

    public sealed override void CheckTransitions(StateMachine machine)
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

    protected virtual void SetAnimation()
    {
        //Debug.LogError($" TRY to SET ANIMATION!");
        //owner.SetAnimationBool(AnimationBools.ISDEAD, true);
        owner.ActivateAnimationTrigger(AnimationTriggers.EMPTY);

        //owner.SetAnimationBool(AnimationBools.ISWALK, !_navMeshAgent.isStopped);
    }
}