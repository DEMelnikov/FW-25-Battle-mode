using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;


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

    [Header("Activation Requirements")]
    [SerializeField] protected List<Trigger>      triggers      = new List<Trigger>();
    [SerializeField] public    List<AbilityCost>  costs         = new List<AbilityCost>();    

    [Header("Targeting:")]
    [SerializeField] protected AbilityTargets _targeting    = AbilityTargets.SELF;
    [SerializeField] 
           [Min (0)] protected float          _targetRadius = 0f;

    [Header("Execution Logic")]
    [SerializeField] private AbilityAction     _abilityAction;
    [SerializeField] private AnimationTriggers _animationTrigger = AnimationTriggers.EMPTY;

    [SerializeField] private List<AbilityResolve>  resolves = new List<AbilityResolve>();

    [Header("UI & Filtering")]
    public List<AbilityTag> abilityTags = new List<AbilityTag>();

    //[Header("Events Subscribing")]
                    private bool isSubscribed = false;

    // UI методы   

    public virtual void IniTialize(ICharacter character)
    {
        _character = character;
        isSubscribed = false;
    }

    public bool HasTag(AbilityTag tag) => abilityTags.Contains(tag);
    public List<AbilityTag> GetTags() => new List<AbilityTag>(abilityTags);


    public string GetName { get=>abilityName; }
    public bool IsSubscribed { get => isSubscribed; set => isSubscribed = value; }
    //public override bool HasTag(AbilityTag tag) => tags.Contains(tag);
    public override IAction GetAbilityAction() => _abilityAction;
    //public override List<AbilityResolve> GetAbilityResolves() => resolves;

    public override bool CanAfford(ICharacter character)
    {
            
        if (logging) { Debug.Log($"{character.GetGameObject.name} - Start check CanAfford at ability {abilityName}"); }
        if (costs.Count==0)return true;

        foreach (var cost in costs)
        {
            if (!cost.CanAffordCost(character))
            {
                if (logging) { Debug.Log($"CanAfford failed у {abilityName}"); }
                return false;
            }
        }
        if (logging) { Debug.Log($"{character.GetGameObject.name} -  CanAfford passed at ability {abilityName}"); }
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
    public bool  GetLoggingState() { return logging; }

    public sealed override bool TryActivateAbility(ICharacter character, out int outcome )
    {
        //string summary = "Character:" + character.ObjName + " execute ability " + this.name;
        ;
        if (logging) { Debug.Log($"-Start Activation Ability {this.name} у {character.ObjName}"); }
         
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

    if (logging) { Debug.Log($"Start Check Triggers у {character.ObjName}"); }
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
