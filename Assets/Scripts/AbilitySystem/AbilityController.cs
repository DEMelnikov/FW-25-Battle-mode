using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class AbilityController : MonoBehaviour, IAbilityController
{
                        private ICharacter     character;
    [SerializeField] private List<IAbility> availableAbilities = new List<IAbility>();

    private void Awake()
    {
        if (character == null)
            character = GetComponent<Character>();
    }
    [ContextMenu("Вызвать метод")]

    public  bool TryActivateAbility(IAbility ability)
    {
        return ability.TryActivateAbility(character, out _); 
    }

    public bool CanActivateAbility(BaseAbility ability)
    {
        return ability != null && ability.CanAfford(character);
    }

    // Методы для управления списком способностей
    public void AddAbility(IAbility ability)
    {
        if (ability != null && !availableAbilities.Contains(ability))
            availableAbilities.Add(ability);
    }

    public void RemoveAbility(IAbility ability)
    {
        availableAbilities.Remove(ability);
    }

    public List<IAbility> GetAbilitiesByTag(string tag)
    {
        return availableAbilities.FindAll(a => a.HasTag(tag));
    }

    public List<IAbility> GetAllAbilities() => availableAbilities;
}
