using UnityEngine;

[CreateAssetMenu(fileName = "HasValidatedEnemy", menuName = "FW25/Triggers/HasValidTarget")]

public class HasValidTargetTrigger : Trigger
{
    //[SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Hero;

    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log($"{character.name} starts CheckTrigger {this.name}");

        // Используем новый безопасный метод
        if (character.GetTargetsVault().TryGetTargetEnemy(out _))
        {
            if (logging) Debug.Log($"{character.name} CheckTrigger {this.name} pass success");
            return true;
        }

        if (logging) Debug.Log($"{character.name} CheckTrigger {this.name} unsuccess");
        return false;
    }
}



