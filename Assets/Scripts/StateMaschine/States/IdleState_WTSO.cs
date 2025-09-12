using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Idle State")]
public class IdleState_WTSO : State
{
    //[SerializeField][TextArea(2, 3)] public string description;
    //[SerializeField] public bool logging = true;
    //[SerializeField] public List<Transition> transitions = new List<Transition>();
    //[SerializeField] public State allTransitionsFailedState;

    public override void OnEnter(IStateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Enter Idle State:");
        base.OnEnter(machine);
    }

    public override void OnExit(IStateMachine machine)
    {
        if (logging) Debug.Log($"{machine.Context.Owner.name} Exit Idle State:");
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {
        if (logging) Debug.Log($"{machine.Context.Owner.name} On Idle State:");
        base.OnUpdate(machine);
    }
}
