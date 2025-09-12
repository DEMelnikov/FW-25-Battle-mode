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
    //    /// ��������� ����������� ��������� �����������
    //    /// </summary>
    //    /// <param name="ability">����������� ��� ��������</param>
    //    /// <returns>True ���� ����������� ����� ���� ������������</returns>
    //    bool CanActivateAbility(IAbility ability);

    //    /// <summary>
    //    /// ��������� ����������� � ������ ���������
    //    /// </summary>
    //    /// <param name="ability">����������� ��� ����������</param>
    //    void AddAbility(IAbility ability);

    //    /// <summary>
    //    /// ������� ����������� �� ������ ���������
    //    /// </summary>
    //    /// <param name="ability">����������� ��� ��������</param>
    //    void RemoveAbility(IAbility ability);

    //    /// <summary>
    //    /// �������� ������ ������������ �� ����
    //    /// </summary>
    //    /// <param name="tag">��� ��� ������</param>
    //    /// <returns>������ ������������ � ��������� �����</returns>
    //    List<IAbility> GetAbilitiesByTag(string tag);

    //    /// <summary>
    //    /// �������� ��� ��������� �����������
    //    /// </summary>
    //    /// <returns>������ ���� ��������� ������������</returns>
    //    List<IAbility> GetAllAbilities();
}

