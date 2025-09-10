using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    public abstract class AbilityResolve : ScriptableObject
    {
        //public abstract void ApplyResolve(Character character, ActionOutcome outcome);
        public abstract void ApplyResolve(Character character, int outcome);

    }
}