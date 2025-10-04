
using NUnit.Framework;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FSMStateCheck", menuName = "FW25/Triggers/States/FSM State check")]

public class FSMStateCheck : Trigger
{
    [SerializeField] private List<FSMStates> _statesToCheck = new();
    [SerializeField] private bool _mustContainState = true; // true Ц требует наличие, false Ц отсутствие

    //private ICharacter _targetCharacter;

    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log($"{character.GetGameObject.name} Start trigger {this.name} ");
        
        var _targetCharacter = GetTargetForTrigger(character);

        var stateToCheck = _targetCharacter.StateMachine.CurrentStateFSM;

        bool contains = _statesToCheck.Contains(stateToCheck);

        // ¬ зависимости от того, требуетс€ совпадение или наоборот
        bool toReturn = _mustContainState ? contains : !contains;
        if (logging) Debug.Log($"END trigger return : {toReturn} ");

        return toReturn;
        
        //Debug.LogError($"{character.GetGameObject.name} TargetEnemyCharGlobalState - no target enemy in Targets Vault");
        //return false;
    }
}



