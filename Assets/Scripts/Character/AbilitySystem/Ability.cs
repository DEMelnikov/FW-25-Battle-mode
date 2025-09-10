using System.Collections.Generic;
using UnityEngine;
using AbilitySystem.AbilityComponents;

namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "New Ability", menuName = "FW25/Ability System/Ability")]
    public class Ability : ScriptableObject
    {
        [Header("Basic Info")]
        [SerializeField] private string abilityName;
        [SerializeField][TextArea(3,5)]  private string description;
                         public Sprite  icon;
        [SerializeField] private bool   logging = false;


        [Header("Activation Requirements")]
        public List<AbilityTrigger> triggers = new List<AbilityTrigger>();
        public List<AbilityCost>    costs    = new List<AbilityCost>();

        [Header("Execution Logic")]
        public AbilityAction        action;
        public List<AbilityResolve> resolves = new List<AbilityResolve>();

        [Header("UI & Filtering")]
        public List<string> tags = new List<string>();

        // UI методы
        public string GetFormattedDescription()
        {
            return $"{description}\n\nCost: {GetCostDescription()}";
        }

        public string GetCostDescription()
        {
            //var costDesc = "";
            //foreach (var cost in costs)
            //{
            //    if (cost.type == AbilityCost.CostType.Stat)
            //    {
            //        costDesc += $"{cost.statName}: {cost.statCost}\n";
            //    }
            //    else
            //    {
            //        costDesc += $"Item {cost.itemId}: {cost.itemCount}\n";
            //    }
            //}
            //return costDesc.Trim();
            return "TODO - work harder";
        }

        public bool HasTag(string tag) => tags.Contains(tag);

        public bool CanAfford(Character character)
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

        public bool PayAllCost(Character character)
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
    }
}