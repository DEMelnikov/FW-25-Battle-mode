using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.TextCore.Text;


[CreateAssetMenu(fileName = "New Ability", menuName = "FW25/Ability System/Ability")]
public class Ability : BaseAbility, IAbility
{
    [Header("Basic Info")]
    [SerializeField] private string abilityName;
    [SerializeField] [TextArea(3,5)] private string description;
                     public Sprite  icon;
    [SerializeField] private bool   logging = false;

    //[Header("Vaults")]
    //[SerializeField] private ActionsVault  _actionsVault;
    //[SerializeField] private TriggersVault _triggersVault;
    
    [Header("Activation Requirements")]
    //[SONameDropdown(typeof(TriggersVault))]
    //public    List<string>   triggersNames = new List<string>();
    [SerializeField] protected List<Trigger>  triggers      = new List<Trigger>();


    [SerializeField] public  List<AbilityCost>     costs    = new List<AbilityCost>();
    //[SerializeField] private IAction               action;


    [Header("Execution Logic")]
    //[SONameDropdown(typeof(ActionsVault))]
    //public string _actionName;
    [SerializeField] private AbilityAction _abilityAction;
    [SerializeField] private AnimationTriggers _animationTrigger = AnimationTriggers.EMPTY;

    [SerializeField] private List<AbilityResolve>  resolves = new List<AbilityResolve>();

    [Header("UI & Filtering")]
    public List<string> tags = new List<string>();

    // UI ������

    public override bool HasTag(string tag) => tags.Contains(tag);
    public override IAction GetAbilityAction() => _abilityAction;
    //public override List<AbilityResolve> GetAbilityResolves() => resolves;

    public override bool CanAfford(ICharacter character)
    {
            
        if (logging) { Debug.Log($"Start check CanAfford � {abilityName}"); }
        if (costs.Count==0)return true;

        foreach (var cost in costs)
        {
            if (!cost.CanAffordCost(character))
            {
                if (logging) { Debug.Log($"CanAfford failed � {abilityName}"); }
                return false;
            }
        }
        if (logging) { Debug.Log($"CanAfford passed � {abilityName}"); }
        return true;
    }

    protected sealed override bool PayAllCost(ICharacter character)
    {
        if (logging) { Debug.Log($"Start PayAllCost  � {abilityName}"); }

        foreach (var cost in costs)
        {
            if (!cost.PayAbilityCost(character))
            {
                return false;
            }
        }
        return true;
    }

    public string GetAbilityName() { return abilityName; }
    public bool GetLoggingState() { return logging; }

    public sealed override bool TryActivateAbility(ICharacter character, out int outcome )
    {
        if (logging) { Debug.Log($"-Start Activation Ability {this.name} � {character.name}"); }
         
        outcome = 0;

        if (!CanAfford(character) || !CheckTriggersReady(character)) return false;

        if (logging) Debug.Log($"Ability {this.name} - ready to PayCost");
        // �������� ���������
        if (!PayAllCost(character)) return false;

        if (logging) Debug.Log($"Ability {this.name} - ready to abilityAction");

        // ���������� ��������
        outcome = _abilityAction.ExecuteAction(character);   //.GetExecuteAction(character);
        character.ActivateAnimationTrigger(_animationTrigger);


        if (logging) Debug.Log($"Ability {this.name} - result = {outcome} successes");

        // ���������� �����������
        foreach (var resolve in resolves)
        {
            resolve.ApplyResolve(character, outcome);
        }

        return true;
    }

protected sealed override bool CheckTriggersReady(ICharacter character)
{

    if (logging) { Debug.Log($"Start Check Triggers � {character.name}"); }
    if (triggers.Count == 0) return true;
    // ��������� ��������
    foreach (var trigger in triggers)
    {
        //if (!trigger.CheckTrigger(character))
        //    return false;
        if (!trigger.CheckTrigger(character))
        {
            if (logging) Debug.Log($"{abilityName} Trigger {trigger.name} not passed");
            return false;
        }
            if (logging) Debug.Log($"  {abilityName} Trigger {trigger.name} passed +++");
    }

    return true;
}

public virtual IAbility Clone()
{
    // ���������� ����� ����� Instantiate
    return Instantiate(this);
}

}
