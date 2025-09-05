
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private List<Ability> startingAbilities = new List<Ability>();

    private Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
    private List<Ability> activeAbilities = new List<Ability>();

    private void Awake()
    {
        if (character == null) character = GetComponent<Character>();
    }

    private void Start()
    {
        InitializeAbilities();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach (var ability in abilities.Values)
        {
            ability.UpdateAbility(deltaTime);
        }
    }

    private void InitializeAbilities()
    {
        foreach (var ability in startingAbilities)
        {
            AddAbility(ability);
        }
    }

    public bool AddAbility(Ability ability)
    {
        if (abilities.ContainsKey(ability.abilityName)) return false;

        var abilityInstance = Instantiate(ability);
        abilityInstance.Initialize(character);
        abilities.Add(abilityInstance.abilityName, abilityInstance);

        if (abilityInstance.type == AbilityType.Passive)
        {
            abilityInstance.Activate(character);
            activeAbilities.Add(abilityInstance);
        }

        return true;
    }

    public bool ActivateAbility(string abilityName)
    {
        if (abilities.TryGetValue(abilityName, out Ability ability))
        {
            if (ability.Activate(character))
            {
                if (ability.isActive)
                {
                    activeAbilities.Add(ability);
                }
                return true;
            }
        }
        return false;
    }

    public AbilityState GetAbilityState(string abilityName)
    {
        return abilities.TryGetValue(abilityName, out Ability ability) ? ability.state : AbilityState.Disabled;
    }

    public List<Ability> GetAbilitiesByTag(string tag)
    {
        var result = new List<Ability>();
        foreach (var ability in abilities.Values)
        {
            if (ability.tags.Contains(tag))
            {
                result.Add(ability);
            }
        }
        return result;
    }
}