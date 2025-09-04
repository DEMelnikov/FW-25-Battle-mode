using UnityEngine;

namespace FW25.Abilities 
{
    public abstract class AbilityEffectSO : ScriptableObject
    {
        public virtual void OnStart(AbilityContext ctx) { }
        public virtual void OnStop(AbilityContext ctx) { }
        public virtual void OnTick(AbilityContext ctx, float dt) { }
    }
}
