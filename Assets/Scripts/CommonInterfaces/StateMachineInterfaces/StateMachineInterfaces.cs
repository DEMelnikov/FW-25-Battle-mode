using UnityEngine;
using UnityEngine.TextCore.Text;

public interface IStateMachine
{
    void SetState(State newState);
    State GetCurrentState();
    IStateContext Context { get; }
}

public interface IStateContext
{
    GameObject Owner { get; }
    ICharacter GetCharacter();
    IAbilityController GetAbilityController();

}

public interface ITransition
{
    Decision decision { get; }
    State trueState { get; }
}

