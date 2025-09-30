using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.TextCore.Text;


[CreateAssetMenu(fileName = "New Ability", menuName = "FW25/Ability System/Ability")]
public class Ability : BaseAbility, IAbility
{
    [Header("Basic Info")]

    [SerializeField] protected string     abilityName;
    [SerializeField] 
    [TextArea(3,5)]  private   string     description;
                     public    Sprite     icon;
    [SerializeField] protected bool       logging = false;
    [SerializeField] protected ICharacter _character;

    [Header("Targeting:")]
    [SerializeField] protected AbilityTargets _targeting    = AbilityTargets.SELF;
    [SerializeField] 
           [Min (0)] protected float          _targetRadius = 0f;

    [Header("Activation Requirements")]
    [SerializeField] protected List<Trigger>    triggers      = new List<Trigger>();
    [SerializeField] public  List<AbilityCost>  costs         = new List<AbilityCost>();    

    [Header("Execution Logic")]
    [SerializeField] private AbilityAction     _abilityAction;
    [SerializeField] private AnimationTriggers _animationTrigger = AnimationTriggers.EMPTY;

    [SerializeField] private List<AbilityResolve>  resolves = new List<AbilityResolve>();

    [Header("UI & Filtering")]
    public List<string> tags = new List<string>();

    [Header("Events Subscribing")]
    [SerializeField] private bool isSubscribed = false;

    // UI методы

    public void IniTialize(ICharacter character)
    {
        //Debug.LogError($"{character.GetGameObject.name} - TRY IniTialize abilitiy");
        _character = character;
        isSubscribed = false;

        //Debug.LogError($"{character.GetGameObject.name} -  IniTialize Success {_character.GetGameObject.name} + {isSubscribed}");
    }

    public string GetName { get=>abilityName; }
    public bool IsSubscribed { get => isSubscribed; set => isSubscribed = value; }
    public override bool HasTag(string tag) => tags.Contains(tag);
    public override IAction GetAbilityAction() => _abilityAction;
    //public override List<AbilityResolve> GetAbilityResolves() => resolves;

    public override bool CanAfford(ICharacter character)
    {
            
        if (logging) { Debug.Log($"Start check CanAfford у {abilityName}"); }
        if (costs.Count==0)return true;

        foreach (var cost in costs)
        {
            if (!cost.CanAffordCost(character))
            {
                if (logging) { Debug.Log($"CanAfford failed у {abilityName}"); }
                return false;
            }
        }
        if (logging) { Debug.Log($"CanAfford passed у {abilityName}"); }
        return true;
    }

    protected sealed override bool PayAllCost(ICharacter character)
    {
        if (logging) { Debug.Log($"Start PayAllCost  у {abilityName}"); }

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
        if (logging) { Debug.Log($"-Start Activation Ability {this.name} у {character.name}"); }
         
        outcome = 0;

        if (!CanAfford(character) || !CheckTriggersReady(character)) return false;

        if (logging) Debug.Log($"Ability {this.name} - ready to PayCost");
        // Списание стоимости
        if (!PayAllCost(character)) return false;

        if (logging) Debug.Log($"Ability {this.name} - ready to abilityAction");

        // Выполнение действия
        if(_abilityAction != null) outcome = _abilityAction.ExecuteAction(character);   //.GetExecuteAction(character);

        character.ActivateAnimationTrigger(_animationTrigger);


        if (logging) Debug.Log($"Ability {this.name} - result = {outcome} successes");

        // Применение результатов
        foreach (var resolve in resolves)
        {
            resolve.ApplyResolve(character, outcome);
        }

        return true;
    }

    protected sealed override bool CheckTriggersReady(ICharacter character)
{

    if (logging) { Debug.Log($"Start Check Triggers у {character.name}"); }
    if (triggers.Count == 0) return true;
    // Проверяем триггеры
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
        // Реализация клона через Instantiate
        return Instantiate(this);
    }

    public virtual void Subscribe(ICharacter character)
    {
        // По умолчанию ничего не подписываем
    }

    public virtual void Unsubscribe(ICharacter character)
    {
        // По умолчанию ничего не отписываем
    }

    protected virtual void EventHandler()
    {
        //TryActivateAbility(character, out int outcome);
    }

    public virtual void OnUpdate(float deltatime)
    {

    }
}
