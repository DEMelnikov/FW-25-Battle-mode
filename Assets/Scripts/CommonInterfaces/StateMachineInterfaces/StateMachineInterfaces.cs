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

}

public interface ITransition
{
    Decision decision { get; }
    State trueState { get; }
}

