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
    //[SerializeField] private List<IAbility> unlockedAbilities = new List<IAbility>(); //TODO - подумать - возможно стоит делать отдельный Vault дл€ каждого персонажа



    public void Initialize()
    {
        
    }



    // ѕубличные свойства дл€ доступа к параметрам
    public IAbility BaseAttackAbility => _defaultAttackAbility;
    public float PursuitDistance => pursuitDistance;

    // ћетоды обновлени€ значений
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
