public interface IStatModifier 
{
    /// <summary>
    /// ���������� ������������� ������������
    /// </summary>
    string Id { get; }

    /// <summary>
    /// �������� ������������ (��� ��� ��� ��������� �����������)
    /// </summary>
    string Source { get; }

    /// <summary>
    /// ������� �������������� ��� �����������
    /// </summary>
    StatTag TargetStat { get; }

    /// <summary>
    /// �������� ������������
    /// </summary>
    float Value { get; }

    /// <summary>
    /// ��� ������������ (����������, ����������������� � �.�.)
    /// </summary>
    StatModType Type { get; }

    void AddToValue(float amount);
}