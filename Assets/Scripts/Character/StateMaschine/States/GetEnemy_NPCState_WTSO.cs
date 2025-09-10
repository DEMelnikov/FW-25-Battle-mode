using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/GetEnemy (NPC)")]
public class GetEnemy_NPCState_WTSO : StateWithTransitions
{
                     private AbilityController _abilityController;
    [SerializeField] private Ability _firstAbilityToSetEnemy;

    public override void OnEnter(StateMachine machine)
    {
        _abilityController = machine.Context._abilityController;
        _abilityController.TryActivateAbility(_firstAbilityToSetEnemy);
        base.OnEnter(machine);
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
        base.OnUpdate(machine);
    }

    protected override void CheckTransitions(StateMachine machine)
    {
        base.CheckTransitions(machine);
    }
}
