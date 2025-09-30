using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

//Version 1.2 - with Dictionary for abilities with subscribe

public class AbilityController : MonoBehaviour, IAbilityController
{
                     private ICharacter     character;
    [SerializeField] private bool logging = false;
    [SerializeField] private List<Ability> AabilitiesATStart = new List<Ability>();
    [SerializeField] private List<IAbility> availableAbilities = new List<IAbility>();          //OLD

    private Dictionary<string, Ability> abilityInstances      = new Dictionary<string, Ability>();
    private Dictionary<string, Ability> timedAbilityInstances = new Dictionary<string, Ability>(); 

    private void Awake()
    {

    }

    private void OnDestroy()
    {
        foreach (var ability in abilityInstances)
        {
            ability.Value.Unsubscribe(character);
        }
    }

    public void Initialize()
    {
        if (character == null)
            character = GetComponent<Character>();

        //Debug.LogError($"{character.GetGameObject.name} - Start Initialize AC");

        //Debug.LogError($"{character.GetGameObject.name} - in AabilitiesATStart {AabilitiesATStart.Count} abilities");

        foreach (var ability in AabilitiesATStart) 
        {        
            InitAbility(ability);  
        }

        //Debug.LogError($"{character.GetGameObject.name} - in abilityInstances {abilityInstances.Count} abilities");

        foreach (var ability in abilityInstances)
        {

            ability.Value.IniTialize(character);
            ability.Value.Subscribe(character);
        }



        if (logging) Debug.Log($" {character.name } AbilityController Initialized = {abilityInstances.Count} abilities");
    }

    private void InitAbility(Ability originalability)
    {
        if (!abilityInstances.ContainsKey(originalability.GetName))
        {
            Ability clone = Instantiate(originalability);
            abilityInstances[originalability.GetName] = clone;
        }
    }

    public Dictionary<string, Ability> GetAbilities { get => abilityInstances; }
    //public bool TryGetAbilityByName(string _abilityName, out var _ability)
    //{
       
    //    if abilityInstances.Co
    //}

    [ContextMenu("Вызвать метод")]

    [System.Obsolete("old")]
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

    //[System.Obsolete("Use HasTargetEnemy() for checking and ValidateAndCleanTarget() for cleaning")] 
    //public List<IAbility> GetAbilitiesByTag(string tag)
    //{
    //    return availableAbilities.FindAll(a => a.HasTag(tag));
    //}

    public List<IAbility> GetAllAbilities() => availableAbilities;
}
