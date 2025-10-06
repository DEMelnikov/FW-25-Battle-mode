using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Temp_AttackState")]
public class Temp_AttackState_WTSO : StateWithPause
{

    private IBehaviorProfile        behaviorProfile;
    private ICharacterTargetsVault  targetsVault;
    private TimerTrigger            attackTimer;


    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        behaviorProfile = owner.BehaviorProfile;
        targetsVault = owner.TargetsVault;

        attackTimer = new TimerTrigger(
        duration: behaviorProfile.BaseAttackInterval, // Например, 1 секунда
        onTick: () => Attack(), //defaultAttack.Use(),        // Активация атаки
        onStart: null,
        onComplete: null,
        looped: true);

        attackTimer.Start();
        Engage();
    }

    public override void OnExit(StateMachine machine)
    {
        attackTimer.UnsubscribeFromPauseEvents();
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(StateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(StateMachine machine)
    {
        base.OnUpdate(machine);

        attackTimer. Update(Time.deltaTime);
        targetsVault.UpdateDistances();

        //Debug.Log($"is running: {attackTimer.IsRunning} GlobalPause {PauseManager.IsPaused}");
        //Debug.Log($"Attack interval Time remain: {attackTimer.RemainingTime} Loops: {attackTimer.LoopsCompleted}");
    }

    private void Attack()
    {
        owner.AbilityController.TryExecuteAttackAbility();
    }

    private void Engage()
    {
        if (!owner.InEngage)
        {
            owner.InEngage = true;
        }
        if (targetsVault.TryGetTargetCharacter(out var targetCharacter))
        {
            if (!targetCharacter.InEngage)
            {
                targetCharacter.UnderMeleAttack(owner);
            }
        }
    }
}
