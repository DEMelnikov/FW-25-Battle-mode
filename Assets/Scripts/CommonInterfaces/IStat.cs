using System;
using System.Collections.Generic;

public interface IStat
{
    /// <summary>
    /// �������� ��������������
    /// </summary>
    string Name { get; }

    /// <summary>
    /// ��� �������������� ��� �������������
    /// </summary>
    StatTag Tag { get; }

    /// <summary>
    /// ������� �������� ��������������
    /// </summary>
    float BaseValue { get; }

    /// <summary>
    /// ������� ��������� �������� ��������������
    /// </summary>
    float Value { get; }

    /// <summary>
    /// ������� ��������� �������� ��������������
    /// </summary>
    event Action<IStat, float, float> OnValueChanged;

    /// <summary>
    /// ������� ��� ���������� �������� ���� ����
    /// </summary>
    event Action<IStat, float> OnBelowZero;

    /// <summary>
    /// ������������� ��������� � ��������� ������� ��������
    /// </summary>
    void CheckForInvoke();

    /// <summary>
    /// ��������� ���������� �����������
    /// </summary>
    void AddModifier(IStatModifier modifier) { }

    /// <summary>
    /// ��������� ��������� �����������
    /// </summary>
    void AddTimedModifier(ITimedStatModifier modifier);

    /// <summary>
    /// ������� ����������� �� ��������������
    /// </summary>
    bool RemoveModifier(string id);

    /// <summary>
    /// ������� ��� ������������ �� ������������� ���������
    /// </summary>
    bool RemoveModifiersFromSource(string source);

    /// <summary>
    /// ��������� ��������� ��������� �������������
    /// </summary>
    void UpdateTimedModifiers(float deltaTime);

    /// <summary>
    /// ������������� ����� ������� ��������
    /// </summary>
    void SetBaseValue(float newBaseValue);

    /// <summary>
    /// ��������� �������� � ���������� ������������
    /// </summary>
    void AddToTmpModifier(float tmpValue);
}