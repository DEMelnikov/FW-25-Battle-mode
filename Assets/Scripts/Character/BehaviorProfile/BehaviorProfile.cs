using System;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

[DisallowMultipleComponent]
public class BehaviorProfile : MonoBehaviour
{
    [Header("Attack Settings:")]
    [SerializeField] private IAbility _defaultAttackAbility;

    [Header("Defence Settings:")]

    [Header("Movement Settings:")]
    [SerializeField] private float pursuitDistance = 150f;


    
    //[Header("Unlocked Abilities")]
    //[SerializeField] private List<IAbility> unlockedAbilities = new List<IAbility>(); //TODO - �������� - �������� ����� ������ ��������� Vault ��� ������� ���������



    public void Initialize()
    {
        
    }



    // ��������� �������� ��� ������� � ����������
    public IAbility BaseAttackAbility => _defaultAttackAbility;
    public float PursuitDistance => pursuitDistance;

    // ������ ���������� ��������
    public void SetBaseAttackAbility(IAbility newAbility)
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
