
using UnityEngine;
namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "HasWP", menuName = "FW25/Triggers/Targets/Has WayPoint")]

    public class HasWayPointNotNull : Trigger
    {
        //[SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Hero;

        public override bool CheckTrigger(ICharacter character)
        {
            if (logging) Debug.Log($"{character.name} starts CheckTrigger {this.name}");

            if (character.GetTargetsVault().GetWayPoint() != Vector3.zero)
                return true;

            return false;
        }
    }
}


