using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Temp_AttackState")]
public class Temp_AttackState_WTSO : StateWithTransitions
{
    public override void OnEnter(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Enter Attack State:");
        base.OnEnter(machine);
    }

    public override void OnExit(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Exit Attack State:");
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(StateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} On Attack State:");
        base.OnUpdate(machine);
    }

    protected override void CheckTransitions(StateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} CheckTransitions start on Attack State:");
        base.CheckTransitions(machine);
    }
}
