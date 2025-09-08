using UnityEngine;
using UnityEngine.TextCore.Text;

public class hState_Idle : State
{
    public hState_Idle(Character character, StateMaschine stateMaschine) : base(character, stateMaschine)
    {
    }

    public override void EnterState()
    {
        Debug.Log($"{base._character.name} enter Idle state");
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        //Debug.Log($"{base._character.name} just Idle");
        if (PauseManager.IsPaused) return;

        if (base._character.GetSelectedTarget() != null)
        {
            Debug.Log($"{base._character.name} just Idle");
            //if (character.IsInAttackRange(character.SelectedTarget))
            //{
            //    stateMachine.ChangeState(character.AttackState);
            //}
            //else
            //{
            //    stateMachine.ChangeState(character.MoveState);
            //}
        }
    }

    public override void PhysicUpdate()
    {
        if (PauseManager.IsPaused) return;
        base.PhysicUpdate();
    }
}


