using AbilitySystem.AbilityComponents;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.TextCore.Text;


    [CreateAssetMenu(fileName = "New Ability", menuName = "FW25/Ability System/Ability")]
    public class Ability : BaseAbility, IAbility
{
        [Header("Basic Info")]
        [SerializeField] private string abilityName;
        [SerializeField][TextArea(3,5)] private string description;
                         public Sprite  icon;
        [SerializeField] private bool   logging = false;


        [Header("Activation Requirements")]
        [SerializeField] public  List<Trigger>  triggers = new List<Trigger>();
        [SerializeField] public  List<AbilityCost>     costs    = new List<AbilityCost>();
        [SerializeField] private AbilityAction         action;
        [SerializeField] private List<AbilityResolve>  resolves = new List<AbilityResolve>();


        [Header("Execution Logic")]

        [Header("UI & Filtering")]
        public List<string> tags = new List<string>();

        // UI методы

        public override bool HasTag(string tag) => tags.Contains(tag);
        public override IAction GetAbilityAction() => action;
        //public override List<AbilityResolve> GetAbilityResolves() => resolves;

    public override bool CanAfford(ICharacter character)
        {
            
            if (logging) { Debug.Log($"Start check CanAfford у {abilityName}"); }
            //Debug.Log()

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
        if (logging) { Debug.Log($"---Start Activation Ability {this.name} у {character.name}"); }
         
        outcome = 0;

        if (!CanAfford(character) || !CheckTriggersReady(character))
            return false;

        Debug.Log("Ability Controller - ready to PayCost");
        // Списание стоимости
        if (!PayAllCost(character))
            return false;

        Debug.Log("Ability Controller - ready to abilityAction");

        // Выполнение действия
         outcome = action.ExecuteAction(character);   //.GetExecuteAction(character);

        Debug.Log($"Ability Controller - result = {outcome} successes");

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
        // Проверяем триггеры
        foreach (var trigger in triggers)
        {
            //if (!trigger.CheckTrigger(character))
            //    return false;
            if (!trigger.CheckTrigger(character))
            {
                Debug.Log($"  {abilityName} Trigger {trigger.name} not passed");
                return false;
            }
            Debug.Log($"  {abilityName} Trigger {trigger.name} passed +++");
        }

        return true;
    }

    public virtual IAbility Clone()
    {
        // Реализация клона через Instantiate
        return Instantiate(this);
    }

}
