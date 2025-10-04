using UnityEngine;

[CreateAssetMenu(fileName = "SetState", menuName = "FW25/Ability System/Resolves/Set FSM State")]
public class SetFSMState : AbilityResolve
{
    //private bool Test;
    //[SerializeField] private State _stateToSet;
    [SerializeField] private FSMStates newState;

    public override void ApplyResolve(ICharacter character, int outcome)
    {

        ICharacter targetCharacter = GetTargetForAbility(character);

        var stateMachine = targetCharacter.StateMachine;
        stateMachine.SetState(newState);

    }
}