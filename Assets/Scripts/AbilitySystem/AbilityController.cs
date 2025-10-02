using System.Collections.Generic;
using Unity.Android.Gradle;
using Unity.VisualScripting;
using UnityEditor.Playables;
using UnityEngine;

//Version 1.2 - with Dictionary for abilities with subscribe

public class AbilityController : MonoBehaviour, IAbilityController
{
                     private ICharacter    character;
    [SerializeField] private bool          logging            = false;
    [SerializeField] private List<Ability> AabilitiesATStart  = new List<Ability>();

    [Header("Attack Settings:")]
    [SerializeField] private Ability _initialAttackability;
                     private Ability _currentAttackAbility;

    [Header("Defence Settings:")]
    [SerializeField] private Ability _initialDefenceAbility;
                     private Ability _currentDefenceAbility;

    private Dictionary<string, Ability> abilityInstances      = new Dictionary<string, Ability>();
    private Dictionary<string, Ability> timedAbilityInstances = new Dictionary<string, Ability>();

    #region UnityMethods
    private void Update()
    {
        foreach (var ability in timedAbilityInstances)
        {
            ability.Value.OnUpdate(Time.deltaTime);
        }
    }


    private void OnDestroy()
    {
        foreach (var ability in abilityInstances)
        {
            ability.Value.Unsubscribe(character);
        }
        foreach (var ability in timedAbilityInstances)
        {
            ability.Value.Unsubscribe(character);
        }
    }
    #endregion

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

        if (_initialAttackability == null || _initialDefenceAbility == null) 
            Debug.LogError($"{this.gameObject.name} - in Ability Controller No Initial attack or defence abilities");
        
        InitAbility(_initialAttackability);
        _currentAttackAbility = abilityInstances[_initialAttackability.GetName];

        InitAbility(_initialDefenceAbility);
        _currentDefenceAbility = abilityInstances[_initialDefenceAbility.GetName];

        if (_currentAttackAbility == null || _currentDefenceAbility == null)
            Debug.LogError($"{this.gameObject.name} - in Ability Controller Attack/Defence abilities not initialized!");


        if (logging) Debug.Log($" {character.ObjName } AbilityController Initialized = {abilityInstances.Count+timedAbilityInstances.Count} abilities");
    }

    private void InitAbility(Ability originalability)
    {
        if (!abilityInstances.ContainsKey(originalability.GetName) || !timedAbilityInstances.ContainsKey(originalability.GetName) )
        {
            Ability clone = Instantiate(originalability);
            clone.IniTialize(character);
            clone.Subscribe(character);

            if (clone.IsTimed)
            {
                timedAbilityInstances[originalability.GetName] = clone;
            }
            else
            {
                abilityInstances[originalability.GetName] = clone;
            }
        }
    }

    public Dictionary<string, Ability> GetAbilities { get => abilityInstances; }
    public Dictionary<string, Ability> GetTimedAbilityInstances { get => timedAbilityInstances; }

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

    //// Методы для управления списком способностей OLD
    //public void AddAbility(IAbility ability)
    //{
    //    if (ability != null && !availableAbilities.Contains(ability) && !timedAbilityInstances.Contains(ability) )
    //        availableAbilities.Add(ability);
    //}

    //public void RemoveAbility(IAbility ability)
    //{
    //    availableAbilities.Remove(ability);
    //}

    //[System.Obsolete("Use HasTargetEnemy() for checking and ValidateAndCleanTarget() for cleaning")] 
    //public List<IAbility> GetAbilitiesByTag(string tag)
    //{
    //    return availableAbilities.FindAll(a => a.HasTag(tag));
    //}

    //public List<IAbility> GetAllAbilities() => availableAbilities;
}
