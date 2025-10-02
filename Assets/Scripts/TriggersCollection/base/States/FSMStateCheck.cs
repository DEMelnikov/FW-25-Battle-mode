
using NUnit.Framework;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "FSMStateCheck", menuName = "FW25/Triggers/States/FSM State check")]

public class FSMStateCheck : Trigger
{
    [SerializeField] private List<FSMStates> _statesToCheck = new List<FSMStates>();
    [SerializeField] private bool _mustContainState = true; // true Ц требует наличие, false Ц отсутствие

    [Header("Target:")]
    [SerializeField] private AbilityTargets _target = AbilityTargets.SELF;

    private ICharacter _targetCharacter;

    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log($"{character.GetGameObject.name} Start trigger {this.name} ");
        
        _targetCharacter = null;

        switch (_target)
        {
            case AbilityTargets.SELF:
                _targetCharacter = character;
                break;

            case AbilityTargets.TARGETENEMY:
                if(!character.TargetsVault.TryGetTargetCharacter(out _targetCharacter))
                {
                    Debug.LogError($"{character.GetGameObject.name} - has no TargetEnemy! ");
                    return false;
                }
                break;

            default:
                Debug.LogError($"{character.GetGameObject.name} - Unknown target for trigger {_target}");
                break;
        }

        var stateToCheck = _targetCharacter.GetStateMachine().CurrentStateFSM;

        bool contains = _statesToCheck.Contains(stateToCheck);

        // ¬ зависимости от того, требуетс€ совпадение или наоборот
        bool toReturn = _mustContainState ? contains : !contains;
        if (logging) Debug.Log($"END trigger return : {toReturn} ");

        return toReturn;
        
        //Debug.LogError($"{character.GetGameObject.name} TargetEnemyCharGlobalState - no target enemy in Targets Vault");
        //return false;
    }
}



