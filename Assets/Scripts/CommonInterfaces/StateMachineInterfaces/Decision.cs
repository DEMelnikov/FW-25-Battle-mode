using AbilitySystem.AbilityComponents;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : ScriptableObject
{
    [SerializeField] [TextArea (2,3)]private string _description;

    [Header("Triggers Vault")]
    public TriggersVault triggersVault;// Ссылка на нужный vault-ассет

    [SerializeField] public bool logging = true;
   
    [SONameDropdown(typeof(TriggersVault))]
    public List<string> triggersVaultNames = new List<string>();
    //public string triggerName;

    [SerializeField] protected List<Trigger> abilityTriggers = new List<Trigger>(); //TODO remove SerializeField
    private void Awake()
    {
        abilityTriggers.Clear();
        Initialize();
    }
    protected void Initialize()
    {
        //if(abilityTriggers.Count > 0) return;

        abilityTriggers.Clear();

        if (triggersVault == null)
        {
            Debug.LogWarning("TriggersVault не назначен");
            return;
        }

        foreach (var triggerName in triggersVaultNames)
        {
            if (string.IsNullOrEmpty(triggerName)) continue;

            var clonedTrigger = triggersVault.GetCopyByName(triggerName);
            if (clonedTrigger != null)
            {
                abilityTriggers.Add(clonedTrigger);
            }
            else
            {
                if (logging)
                    Debug.LogWarning($"В triggersVault не найден триггер с именем '{triggerName}'");
            }
        }
    }
    public virtual void OnEnter(IStateMachine machine) { }
    public virtual void OnExit(IStateMachine machine) { }
    public virtual void OnUpdate(IStateMachine machine) { }
    public virtual bool Decide(IStateMachine machine) { return false; }
}