using UnityEngine;

//TODO - move class to Triggers Vault
public abstract class Trigger : ScriptableObject
    {

        [SerializeField] [TextArea(3, 5)] protected string _description;
        [SerializeField] string _name;

        [Header("Настройки:")]
                [SerializeField] protected bool logging = true;
        public abstract bool CheckTrigger(ICharacter character);
        public string GetTriggerName() { return _name; } //TODO - seal this

    public virtual Trigger Clone() //TODO - seal this
        {
            // Реализация клона через Instantiate
            return Instantiate(this);
        }
    //public bool GetLoggingState() {  return logging; }
}
