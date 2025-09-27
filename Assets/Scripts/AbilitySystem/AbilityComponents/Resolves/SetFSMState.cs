using UnityEngine;

[CreateAssetMenu(fileName = "SetState", menuName = "FW25/Ability System/Resolves/Set FSM State")]
public class SetFSMState : AbilityResolve
{
    //private bool Test;
    [SerializeField] private State _stateToSet;

    public override void ApplyResolve(ICharacter character, int outcome)
    {
        if(_stateToSet == null) return;

        var stateMachine = character.GetStateMachine();
        stateMachine.SetState(_stateToSet);

    }
}