using AbilitySystem;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/GetEnemy (NPC)")]
public class GetEnemy_NPCState_WTSO : State
{
    //[SerializeReference] private IAbility _firstAbilityToSetEnemy;
    //[SerializeField] private ScriptableObject _abilityObject;
    [SerializeField] private IAbility _firstAbilityToSetEnemy;

    [Header("Визуальный поиск цели:")]
    [AbilityName]  public string abilityName;
    private IAbility runtimeAbility;


    private IAbilityController _abilityController; //nah ???

    //[SerializeField][TextArea(2, 3)] public string description;
    ////[SerializeField] public bool logging = true;
    //[SerializeField] public List<Transition> transitions = new List<Transition>();
    //[SerializeField] public State allTransitionsFailedState;

    public override void OnEnter(IStateMachine machine)
    {
        //runtimeAbility = AbilitiesVault.Instance.GetAbilityCopyByName(abilityName);
        //if (runtimeAbility == null) Debug.LogWarning("runtimeAbility empty");
        _firstAbilityToSetEnemy = runtimeAbility as IAbility;


        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Enter GetEnemy_NPCState_WTSO State:");
        _abilityController = machine.Context.GetAbilityController();
        if (_firstAbilityToSetEnemy == null ) Debug.LogWarning("Ability not SET!!!");
        if (logging) Debug.Log($"{machine.Context.Owner.name} has ability {_firstAbilityToSetEnemy.GetAbilityName()}");

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
