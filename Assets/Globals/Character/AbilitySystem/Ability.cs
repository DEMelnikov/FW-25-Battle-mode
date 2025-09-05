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
        [TextArea]       private string description;
                         public Sprite  icon;


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
            foreach (var cost in costs)
            {
                if (!cost.CanAffordCost(character))
                {
                      return false;
                }
            }
            return true;
        }

        public bool PayAllCost(Character character)
        {
            foreach (var cost in costs)
            {
                if (!cost.PayAbilityCost(character))
                {
                    return false;
                }
            }
            return true;
        }
    }
}