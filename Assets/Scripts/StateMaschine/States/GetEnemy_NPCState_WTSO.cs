using AbilitySystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/GetEnemy (NPC)")]
public class GetEnemy_NPCState_WTSO : State
{
                     private IAbilityController _abilityController;
    [SerializeField] private IAbility _firstAbilityToSetEnemy;

    //[SerializeField][TextArea(2, 3)] public string description;
    ////[SerializeField] public bool logging = true;
    //[SerializeField] public List<Transition> transitions = new List<Transition>();
    //[SerializeField] public State allTransitionsFailedState;

    public override void OnEnter(IStateMachine machine)
    {
        _abilityController = machine.Context.GetAbilityController();
        _abilityController.TryActivateAbility(_firstAbilityToSetEnemy);
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
        CheckTransitions(machine);
        base.OnUpdate(machine);
    }
}
