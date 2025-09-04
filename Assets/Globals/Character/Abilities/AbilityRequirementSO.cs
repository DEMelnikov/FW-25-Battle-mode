using UnityEngine;

namespace FW25.Abilities
{
    public abstract class AbilityRequirementSO : ScriptableObject
    {
        public abstract bool IsMet(Character character, out string reason);
    }
}