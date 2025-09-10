using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    public abstract class AbilityTrigger : ScriptableObject
    {

        [SerializeField] [TextArea(3, 5)] protected string _description;
        [Header("Настройки:")]
                [SerializeField] protected bool logging = true;
        public abstract bool CheckTrigger(ICharacter character);

        //public bool GetLoggingState() {  return logging; }
    }
}