using System.Collections.Generic;
using AbilitySystem.AbilityComponents;
using Unity.VisualScripting;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityController : MonoBehaviour
    {
                         private Character     character;
        [SerializeField] private List<Ability> availableAbilities = new List<Ability>();

        private void Awake()
        {
            if (character == null)
                character = GetComponent<Character>();
        }
        [ContextMenu("Вызвать метод")]
        public bool TryActivateAbility(Ability ability)
        {
            if (ability == null || !CanActivateAbility(ability) || !CheckTriggersReady(ability))
                return false;

            Debug.Log("Ability Controller - ready to PayCost");
            // Списание стоимости
            if (!PayAbilityCost(ability))
                return false;

            Debug.Log("Ability Controller - ready to abilityAction");
            // Выполнение действия
            var outcome = ability.action.ExecuteAction(character);
            
            Debug.Log($"Ability Controller - result = {outcome} successes");

            // Применение результатов
            foreach (var resolve in ability.resolves)
            {
                resolve.ApplyResolve(character, outcome);
            }

            return true;
        }

        public bool CanActivateAbility(Ability ability)
        {
            return ability != null && ability.CanAfford(character);
        }

        private bool CheckTriggersReady(Ability ability)
        {
            if (ability.GetLoggingState()) { Debug.Log($"Start Check Triggers у {ability.name}"); }
            // Проверяем триггеры
            foreach (var trigger in ability.triggers)
            {
                //if (!trigger.CheckTrigger(character))
                //    return false;
                if (!trigger.CheckTrigger(character))
                {
                    Debug.Log($"  {ability.name} Trigger {trigger.name} not passed");
                    return false; 
                }
                Debug.Log($"  {ability.name} Trigger {trigger.name} passed +++");
            }

            return true;
        }

        private bool PayAbilityCost(Ability ability)
        {
            if (!ability.PayAllCost(character)) return false;

            return true;
        }

        // Методы для управления списком способностей
        public void AddAbility(Ability ability)
        {
            if (ability != null && !availableAbilities.Contains(ability))
                availableAbilities.Add(ability);
        }

        public void RemoveAbility(Ability ability)
        {
            availableAbilities.Remove(ability);
        }

        public List<Ability> GetAbilitiesByTag(string tag)
        {
            return availableAbilities.FindAll(a => a.HasTag(tag));
        }

        public List<Ability> GetAllAbilities() => availableAbilities;
    }
}