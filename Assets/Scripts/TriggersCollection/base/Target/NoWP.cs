
using UnityEngine;


[CreateAssetMenu(fileName = "HasNoWP", menuName = "FW25/Triggers/Targets/Has No WayPoint")]

public class NoWP : Trigger
{
    //[SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Hero;

    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log($"{character.name} starts CheckTrigger {this.name}");

        if (character.GetTargetsVault().GetWayPoint() == Vector3.zero)
            return true;

        return false;
    }
}



