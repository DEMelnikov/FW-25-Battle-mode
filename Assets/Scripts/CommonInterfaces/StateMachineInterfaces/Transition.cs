using UnityEngine;

[System.Serializable]
public class Transition
{
    [SerializeField] public Decision decision;
    [SerializeField] public State trueState;
    [SerializeField] private bool isInfluenceCharacterGlobalGoal = false;
    [SerializeField] private CharacterGlobalState setNewGlobalGoal = CharacterGlobalState.Idle;
    [SerializeField] private bool setIdleState = false; 

    public bool IsInfluenceCharacterGlobalGoal { get => isInfluenceCharacterGlobalGoal;}
    public CharacterGlobalState GetNewGlobalGoal { get => setNewGlobalGoal; }
    public bool SetIdleState { get => setIdleState; }
}