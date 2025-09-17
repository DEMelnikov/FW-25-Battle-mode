using AbilitySystem;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/GetEnemy (NPC)")]
public class GetEnemy_NPCState_WTSO : State
{
    //[SerializeReference] private IAbility _firstAbilityToSetEnemy;
    //[SerializeField] private ScriptableObject _abilityObject;

    [Header("Визуальный поиск цели:")]
    [SerializeField] private Ability _firstAbilityToSetEnemy;

    [Header("State when Success")]
    [SerializeField] private State _stateAfterSuccessSearch;

    private IAbilityController _abilityController; //nah ???

    public override void OnEnter(IStateMachine machine)
    {
        base.OnEnter(machine);

        if (_firstAbilityToSetEnemy == null)
        {
            Debug.LogError($"{machine.Context.Owner.name} in State GetEnemy_NPCState_WTSO - Ability not SET!!!");
            return;
        }

        Character character = machine.Context.Owner.GetComponent<Character>();

        if(_firstAbilityToSetEnemy.TryActivateAbility(character, out _))
        {
            machine.SetState(_stateAfterSuccessSearch);
        }
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
