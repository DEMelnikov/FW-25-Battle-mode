using UnityEngine;

[System.Serializable]
public class Transition
{
    [SerializeField] public Decision decision;
    [SerializeField] public State trueState;
    [SerializeField] private bool isInfluenceCharacterGlobalGoal = false;
    [SerializeField] private CharacterGlobalGoal setNewGlobalGoal = CharacterGlobalGoal.Idle;
    [SerializeField] private bool setIdleState = false; 

    public bool IsInfluenceCharacterGlobalGoal { get => isInfluenceCharacterGlobalGoal;}
    public CharacterGlobalGoal GetNewGlobalGoal { get => setNewGlobalGoal; }
    public bool SetIdleState { get => setIdleState; }
}