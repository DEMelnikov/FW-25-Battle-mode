using UnityEngine;

public class StateMaschine
{
    public State CurrentState { get; set; }
    public void Initialize(State startingState)
    {
        CurrentState = startingState;
        CurrentState.EnterState();
    }

    public void ChangeState(State newState)
    {
        CurrentState.ExitState();
        CurrentState = newState;
        CurrentState.EnterState();
    }
}
