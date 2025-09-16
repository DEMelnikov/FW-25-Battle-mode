using System;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[DisallowMultipleComponent]
public class BehaviorProfile : MonoBehaviour
{
    [Header("Vaults:")]
    [SerializeField] private AbilitiesVault abilitiesVault;



    [Header("Attack Settings:")]
    [SONameDropdown(typeof(AbilitiesVault))]
    public string abilityName;

    [SerializeField] private Ability _defaultAttackAbility;

    [Header("Defence Settings:")]

    [Header("Movement Settings:")]
    [SerializeField] [Min(0.1f)] private float pursuitDistance = 150f;



    //[Header("Unlocked Abilities")]
    //[SerializeField] private List<IAbility> unlockedAbilities = new List<IAbility>(); //TODO - �������� - �������� ����� ������ ��������� Vault ��� ������� ���������

    private void Awake()
    {
        if (_defaultAttackAbility == null) Initialize();
    }

    public void Initialize()
    {
        if (abilitiesVault == null || string.IsNullOrEmpty(abilityName))
        {
            Debug.LogError("abilitiesVault or abilityName �� ��������");
            return;
        }

        SetBaseAttackAbility(abilitiesVault.GetCopyByName(abilityName));
    }



    // ��������� �������� ��� ������� � ����������
    public IAbility BaseAttackAbility => _defaultAttackAbility;
    public float PursuitDistance => pursuitDistance;

    // ������ ���������� ��������
    public void SetBaseAttackAbility(Ability newAbility)
    {
        _defaultAttackAbility = newAbility;
    }

    public void SetPursuitDistance(float distance)
    {
        pursuitDistance = Mathf.Max(0f, distance);
    }


    //public void UnlockAbility(Ability ability)
    //{
    //    if (ability != null && !unlockedAbilities.Contains(ability))
    //        unlockedAbilities.Add(ability);
    //}

    //public void LockAbility(Ability ability)
    //{
    //    if (ability != null && unlockedAbilities.Contains(ability))
    //        unlockedAbilities.Remove(ability);
    //}
}
