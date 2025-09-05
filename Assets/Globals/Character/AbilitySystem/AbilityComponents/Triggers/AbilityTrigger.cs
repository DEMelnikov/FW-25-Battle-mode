using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    public abstract class AbilityTrigger : ScriptableObject
    {
        public abstract bool CheckTrigger(Character character);
    }
}