using AbilitySystem.AbilityComponents;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEditor.Playables;
using UnityEngine;

public abstract class IAbility : ScriptableObject
{
    public abstract bool HasTag(string tag);
    public abstract bool CanAfford(ICharacter character);
    public abstract bool PayAllCost(ICharacter character);
    public AbilityAction action;
    public List<AbilityResolve> resolves;
    public List<AbilityTrigger> triggers;
    public virtual bool GetLoggingState() { return false; }
    public string GetAbilityName() { return ""; }
}

public interface IAbilityController
{
    bool TryActivateAbility(IAbility ability);

    //    /// <summary>
    //    /// ѕровер€ет возможность активации способности
    //    /// </summary>
    //    /// <param name="ability">—пособность дл€ проверки</param>
    //    /// <returns>True если способность может быть активирована</returns>
    //    bool CanActivateAbility(IAbility ability);

    //    /// <summary>
    //    /// ƒобавл€ет способность в список доступных
    //    /// </summary>
    //    /// <param name="ability">—пособность дл€ добавлени€</param>
    //    void AddAbility(IAbility ability);

    //    /// <summary>
    //    /// ”дал€ет способность из списка доступных
    //    /// </summary>
    //    /// <param name="ability">—пособность дл€ удалени€</param>
    //    void RemoveAbility(IAbility ability);

    //    /// <summary>
    //    /// ѕолучает список способностей по тегу
    //    /// </summary>
    //    /// <param name="tag">“ег дл€ поиска</param>
    //    /// <returns>—писок способностей с указанным тегом</returns>
    //    List<IAbility> GetAbilitiesByTag(string tag);

    //    /// <summary>
    //    /// ѕолучает все доступные способности
    //    /// </summary>
    //    /// <returns>—писок всех доступных способностей</returns>
    //    List<IAbility> GetAllAbilities();
}

