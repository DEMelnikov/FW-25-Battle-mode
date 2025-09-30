using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    void Initialize();
    void SetState(FSMStates newState);
    CharacterGlobalState CharacterGoal { get; set; }
    GameObject GetGameObject();
}


