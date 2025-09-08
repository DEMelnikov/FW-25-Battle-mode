using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    public abstract class AbilityTrigger : ScriptableObject
    {
        //private bool logging = true;
        public abstract bool CheckTrigger(Character character);

        //public bool GetLoggingState() {  return logging; }
    }
}