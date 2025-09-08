using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Idle State")]
public class IdleState_WTSO : StateWithTransitions
{
    public override void OnEnter(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Enter Idle State:");
        base.OnEnter(machine);
    }

    public override void OnExit(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Exit Idle State:");
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(StateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} On Idle State:");
        base.OnUpdate(machine);
    }

    protected override void CheckTransitions(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} CheckTransitions start on Idle State:");
        base.CheckTransitions(machine);
    }
}
