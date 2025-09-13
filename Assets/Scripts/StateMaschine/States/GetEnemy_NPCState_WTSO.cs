using AbilitySystem;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/GetEnemy (NPC)")]
public class GetEnemy_NPCState_WTSO : State
{
    //[SerializeReference] private IAbility _firstAbilityToSetEnemy;
    [SerializeField] private ScriptableObject _abilityObject;
    private IAbility _firstAbilityToSetEnemy;


                     private IAbilityController _abilityController; //nah ???

    //[SerializeField][TextArea(2, 3)] public string description;
    ////[SerializeField] public bool logging = true;
    //[SerializeField] public List<Transition> transitions = new List<Transition>();
    //[SerializeField] public State allTransitionsFailedState;

    private void Awake()
    {
        // Простое преобразование ScriptableObject в IAbility
        if (_abilityObject is IAbility ability)
        {
            _firstAbilityToSetEnemy = ability;
        }
        else if (_abilityObject != null)
        {
            Debug.LogWarning($"{_abilityObject.name} does not implement IAbility interface", this);
        }
    }

    public override void OnEnter(IStateMachine machine)
    {
        if (logging) Debug.LogWarning($"{machine.Context.Owner.name} Enter GetEnemy_NPCState_WTSO State:");
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
