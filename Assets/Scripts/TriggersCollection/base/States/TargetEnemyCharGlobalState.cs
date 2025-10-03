using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TargetEnemyState", menuName = "FW25/Triggers/States/Target Enemy Character Global State")]

public class TargetEnemyCharGlobalState : Trigger
{
    [SerializeField] private List<CharacterGlobalState> _statesToCheck = new List<CharacterGlobalState>();
    [SerializeField] private bool _mustContainState = true; // true Ц требует наличие, false Ц отсутствие


    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log($"Start trigger {this.name} ");
        if (character.TargetsVault.TryGetTargetCharacter(out var enemy))
        {
            var enemyState = enemy.GetStateMachine().CharacterGoal;
            bool contains = _statesToCheck.Contains(enemyState);

            // ¬ зависимости от того, требуетс€ совпадение или наоборот
            bool toReturn = _mustContainState ? contains : !contains;
            if (logging) Debug.Log($"END trigger return : {toReturn} ");

            return toReturn;
        }
        Debug.LogError($"{character.GetGameObject.name} TargetEnemyCharGlobalState - no target enemy in Targets Vault");
        return false;
    }    
}



