using System.Collections.Generic;
using AbilitySystem.AbilityComponents;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private Character character;
        [SerializeField] private List<Ability> availableAbilities = new List<Ability>();

        private void Awake()
        {
            if (character == null)
                character = GetComponent<Character>();
        }

        public bool TryActivateAbility(Ability ability)
        {
            if (ability == null || !CanActivateAbility(ability))
                return false;

            // ��������� ��������
            foreach (var trigger in ability.triggers)
            {
                if (!trigger.CheckTrigger(character))
                    return false;
            }

            // �������� ���������
            if (!PayAbilityCost(ability))
                return false;

            // ���������� ��������
            var outcome = ability.action.ExecuteAction(character);

            // ���������� �����������
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

        private bool PayAbilityCost(Ability ability)
        {
            foreach (var cost in ability.costs)
            {
                if (cost.type == AbilityCost.CostType.Stat)
                {
                    //TODO
                    //var stat = character.Stats.GetStat(cost.statName);
                    //if (stat != null)
                    //{
                    //    stat.CurrentValue -= cost.statCost;
                    //}
                }
                // ��� ��������� - ��������
                else if (cost.type == AbilityCost.CostType.Item)
                {
                    // � �������: �������� �������� �� ���������
                    Debug.Log($"Used item {cost.itemId} x{cost.itemCount}");
                }
            }
            return true;
        }

        // ������ ��� ���������� ������� ������������
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