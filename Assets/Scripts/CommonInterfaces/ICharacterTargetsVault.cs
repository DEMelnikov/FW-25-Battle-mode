using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public interface ICharacterTargetsVault
{
    public bool HasTargetEnemy();
    public bool TryGetTargetEnemy(out GameObject targetEnemy);
    public bool TryGetTargetCharacter(out ICharacter targetCharacter);
    public GameObject GetTargetEnemy();

}
