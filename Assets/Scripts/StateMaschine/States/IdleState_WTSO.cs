using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Idle State")]
public class IdleState_WTSO : State
{
    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        SetAnimation();


    }

    public override void OnExit(StateMachine machine)
    {
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(StateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(StateMachine machine)
    {
        if (logging) Debug.Log($"{machine.GetGameObject().name} On Idle State:");
        base.OnUpdate(machine);
    }

    protected override void SetAnimation()
    {
        owner.ActivateAnimationTrigger(AnimationTriggers.TOIDLE);
    }
}
