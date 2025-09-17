using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Idle State")]
public class IdleState_WTSO : State
{
    public override void OnEnter(IStateMachine machine)
    {
        base.OnEnter(machine);
    }

    public override void OnExit(IStateMachine machine)
    {
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
