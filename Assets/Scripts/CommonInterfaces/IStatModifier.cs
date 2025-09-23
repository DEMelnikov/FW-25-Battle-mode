public interface IStatModifier 
{
    /// <summary>
    /// Уникальный идентификатор модификатора
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Источник модификатора (кто или что применило модификатор)
    /// </summary>
    string Source { get; }

    /// <summary>
    /// Целевая характеристика для модификации
    /// </summary>
    StatTag TargetStat { get; }

    /// <summary>
    /// Значение модификатора
    /// </summary>
    float Value { get; }

    /// <summary>
    /// Тип модификатора (аддитивный, мультипликативный и т.д.)
    /// </summary>
    StatModType Type { get; }

    void AddToValue(float amount);
}