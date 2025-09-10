using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    public abstract class AbilityResolve : ScriptableObject
    {
        //public abstract void ApplyResolve(Character character, ActionOutcome outcome);
        public virtual void ApplyResolve(ICharacter character, int outcome) { }

    }
}