using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "FW25/State Machine/States/GetEnemy (NPC)")]
public class GetEnemy_NPCState_WTSO : State
{
    //[SerializeReference] private IAbility _firstAbilityToSetEnemy;
    //[SerializeField] private ScriptableObject _abilityObject;

    [Header("Визуальный поиск цели:")]
    [SerializeField] private Ability _firstAbilityToSetEnemy;

    //[Header("State when Success")]
    //[SerializeField] private State _stateAfterSuccessSearch;

    //private IAbilityController _abilityController; //nah ???
    private Character character;

    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);

        if (_firstAbilityToSetEnemy == null)
        {
            Debug.LogError($"{machine.Context.Owner.name} in State GetEnemy_NPCState_WTSO - Ability not SET!!!");
            return;
        }

        character = machine.Context.Owner.GetComponent<Character>();
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
        _firstAbilityToSetEnemy.TryActivateAbility(character, out _);
        base.OnUpdate(machine);
    }
}
