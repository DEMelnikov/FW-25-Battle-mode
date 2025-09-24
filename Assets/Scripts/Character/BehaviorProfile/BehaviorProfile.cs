using System;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[DisallowMultipleComponent]
public class BehaviorProfile : MonoBehaviour, IBehaviorProfile
{
    //[Header("Vaults:")]
    //[SerializeField] private AbilitiesVault abilitiesVault;

    [Header("Attack Settings:")]
    //[SONameDropdown(typeof(AbilitiesVault))]
    //public string defaultAttackAbilityName;
    [SerializeField][Min(0.1f)] private float _weaponRange = 3f;

    [SerializeField] private Ability _defaultAttackAbility;
    [SerializeField] private float    _baseAttackInterval = 2f;

    [Header("Defence Settings:")]

    [Header("Movement Settings:")]
    [SerializeField] [Min(0.1f)] private float pursuitDistance = 150f;
    [SerializeField] [Min(0.1f)] private float walkEnergyCost = 0.5f;



    //[Header("Unlocked Abilities")]
    //[SerializeField] private List<IAbility> unlockedAbilities = new List<IAbility>(); //TODO - подумать - возможно стоит делать отдельный Vault для каждого персонажа

    //private void Awake()
    //{
    //    if (_defaultAttackAbility == null) Initialize();
    //}

    //private void Initialize()
    //{
    //    if (abilitiesVault == null || string.IsNullOrEmpty(defaultAttackAbilityName))
    //    {
    //        Debug.LogError($"{this.gameObject.name}.BehaviorProfile abilitiesVault or abilityName не назначен");
    //        return;
    //    }

    //    SetBaseAttackAbility(abilitiesVault.GetCopyByName(defaultAttackAbilityName));
    //}



    // Публичные свойства для доступа к параметрам
    public Ability BaseAttackAbility  { get => _defaultAttackAbility; set => _defaultAttackAbility = value; }
    public float PursuitDistance => pursuitDistance;

    public float WeaponRange { get => _weaponRange; set => _weaponRange = value; }
    public float BaseAttackInterval { get => _baseAttackInterval; set => _baseAttackInterval = value; }
    public float WalkEnergyCost { get => walkEnergyCost; set => walkEnergyCost = value; }

    // Методы обновления значений
    [System.Obsolete("Use BaseAttackAbility")]
    public void SetBaseAttackAbility(Ability newAbility)
    {
        _defaultAttackAbility = newAbility;
    }

    public void SetPursuitDistance(float distance)
    {
        pursuitDistance = Mathf.Max(0f, distance);
    }

}
