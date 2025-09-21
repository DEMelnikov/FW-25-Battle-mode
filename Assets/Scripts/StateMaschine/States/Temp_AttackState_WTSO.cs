using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Temp_AttackState")]
public class Temp_AttackState_WTSO : StateWithPause
{

    private IBehaviorProfile  behaviorProfile;
    private IAbility          defaultAttack;
    private TimerTrigger      attackTimer;


    public override void OnEnter(IStateMachine machine)
    {
        base.OnEnter(machine);
        behaviorProfile = owner.GetBehaviorProfile();
        defaultAttack = behaviorProfile.BaseAttackAbility;

        attackTimer = new TimerTrigger(
        duration: behaviorProfile.BaseAttackInterval, // Например, 1 секунда
        onTick: () => Attack(), //defaultAttack.Use(),        // Активация атаки
        onStart: null,
        onComplete: null,
        looped: true);

        attackTimer.Start();
    }

    public override void OnExit(IStateMachine machine)
    {
        attackTimer.UnsubscribeFromPauseEvents();
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {
        base.OnUpdate(machine);

        attackTimer.Update(Time.deltaTime);

        //Debug.Log($"is running: {attackTimer.IsRunning} GlobalPause {PauseManager.IsPaused}");
        //Debug.Log($"Attack interval Time remain: {attackTimer.RemainingTime} Loops: {attackTimer.LoopsCompleted}");
    }

    private void Attack()
    {
        if (defaultAttack == null)
        {
            Debug.LogError($" No Default attack {owner.name}");
            return;
        } 
        if (defaultAttack.TryActivateAbility(owner,out var outcome))
        {
           if(logging) Debug.Log($"{this.owner.name} Making Attack result {outcome}");
            return;
        }
        Debug.Log($"{this.owner.name} Making Attack no success");
    }
}
