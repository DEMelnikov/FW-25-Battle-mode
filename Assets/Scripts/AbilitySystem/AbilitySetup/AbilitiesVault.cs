using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilitiesVault")]
public class AbilitiesVault : ScriptableObject
{
    [SerializeField] public List<Ability> abilities;
    public static AbilitiesVault Instance { get; private set; }

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple AbilitiesVault instances found. Only one Instance is supported.");
        }
        else
        {
            Instance = this;
        }
    }

    public IAbility GetAbilityByName(string name)
    {
        return abilities.FirstOrDefault(a => a.GetAbilityName() == name);
    }

    public IAbility GetAbilityCopyByName(string name)
    {
        var original = GetAbilityByName(name);
        if (original != null)
        {
            return original.Clone();
        }
        return null;
    }
}
