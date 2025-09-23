using System;
using System.Collections.Generic;

public interface IStat
{
    /// <summary>
    /// Название характеристики
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Тег характеристики для категоризации
    /// </summary>
    StatTag Tag { get; }

    /// <summary>
    /// Базовое значение характеристики
    /// </summary>
    float BaseValue { get; }

    /// <summary>
    /// Текущее расчетное значение характеристики
    /// </summary>
    float Value { get; }

    /// <summary>
    /// Событие изменения значения характеристики
    /// </summary>
    event Action<IStat, float, float> OnValueChanged;

    /// <summary>
    /// Событие при достижении значения ниже нуля
    /// </summary>
    event Action<IStat, float> OnBelowZero;

    /// <summary>
    /// Принудительно проверяет и вычисляет текущее значение
    /// </summary>
    void CheckForInvoke();

    /// <summary>
    /// Добавляет постоянный модификатор
    /// </summary>
    void AddModifier(IStatModifier modifier) { }

    /// <summary>
    /// Добавляет временный модификатор
    /// </summary>
    void AddTimedModifier(ITimedStatModifier modifier);

    /// <summary>
    /// Удаляет модификатор по идентификатору
    /// </summary>
    bool RemoveModifier(string id);

    /// <summary>
    /// Удаляет все модификаторы от определенного источника
    /// </summary>
    bool RemoveModifiersFromSource(string source);

    /// <summary>
    /// Обновляет состояние временных модификаторов
    /// </summary>
    void UpdateTimedModifiers(float deltaTime);

    /// <summary>
    /// Устанавливает новое базовое значение
    /// </summary>
    void SetBaseValue(float newBaseValue);

    /// <summary>
    /// Добавляет значение к временному модификатору
    /// </summary>
    void AddToTmpModifier(float tmpValue);
}