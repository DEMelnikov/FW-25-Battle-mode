using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject
{
    [SerializeField] [TextArea (2,3)]private string _description;

    //[Header("Triggers Vault")]
    //public TriggersVault triggersVault;// —сылка на нужный vault-ассет

    [SerializeField] public bool logging = true;
   
    //[SONameDropdown(typeof(TriggersVault))]
    //public List<TriggerNameRef> triggersVaultNames = new List<TriggerNameRef>();

    //public string triggerName;

    [SerializeField] protected List<Trigger> abilityTriggers = new List<Trigger>(); //TODO remove SerializeField

    public virtual void OnEnter(IStateMachine machine) { }
    public virtual void OnExit(IStateMachine machine) { }
    public virtual void OnUpdate(IStateMachine machine) { }
    public virtual bool Decide(IStateMachine machine) { return false; }
}