using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Temp_AttackState")]
public class Temp_AttackState_WTSO : State
{
    public override void OnEnter(IStateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Enter Attack State:");
        base.OnEnter(machine);
    }

    public override void OnExit(IStateMachine machine)
    {
        if (logging) Debug.Log($"{machine.Context.Owner.name} Exit Attack State:");
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {
        if (logging) Debug.Log($"{machine.Context.Owner.name} On Attack State:");
        base.OnUpdate(machine);
    }

}
