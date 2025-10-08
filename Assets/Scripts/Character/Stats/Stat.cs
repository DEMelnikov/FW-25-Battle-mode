using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
//using System.Diagnostics;
using UnityEngine;

public class Stat : IStat
{
    public string Name { get; private set; }
    public StatTag Tag { get; private set; }

    public bool LimitValueGreaterBase  { get; private set;}
    private float _initialBaseValue;

    private Func<float> _dynamicBaseValueFormula;
    private float _baseValue;

    private float _lastCalculatedValue;
    private bool _isDirty = true;
    private bool _alarmIfBelowZero = true;

    private List<IStatModifier> _permanentModifiers = new List<IStatModifier>();
    private List<ITimedStatModifier> _timedModifiers = new List<ITimedStatModifier>();

    public event Action<IStat, float, float> OnValueChanged;
    public event Action<IStat, float> OnBelowZero;

    public float Value
    {
        get
        {
            //if (_isDirty)
            //{
            //    float oldValue = _lastCalculatedValue;
            //    _lastCalculatedValue = CalculateFinalValue();
            //    _isDirty = false;
            //    OnValueChanged?.Invoke(this, oldValue, _lastCalculatedValue);
            //}
            CheckForInvoke();

            // Ограничение значения не выше BaseValue, если установлено
            if (LimitValueGreaterBase && _lastCalculatedValue > BaseValue)
            {
                //_lastCalculatedValue = BaseValue;
                _isDirty = true;
                CheckForInvoke();
            }

            return _lastCalculatedValue;
        }
    }

    public float BaseValue
    {
        get => _dynamicBaseValueFormula != null ? _dynamicBaseValueFormula() : _baseValue;
        private set { _baseValue = value; _isDirty = true; }
    }

    public void SetDynamicBaseValueFormula(Func<float> formula)
    {
        _dynamicBaseValueFormula = formula;
        _isDirty = true;
    }

    public void CheckForInvoke()
    {
        if (_isDirty)
        {
            float oldValue = _lastCalculatedValue;
            _lastCalculatedValue = CalculateFinalValue();
            _isDirty = false;

            if (oldValue != _lastCalculatedValue)
            {
                //Debug.l
                //Debug.Log("Should be invoke");
                OnValueChanged?.Invoke(this, oldValue, _lastCalculatedValue);
            }

            if (_alarmIfBelowZero && _lastCalculatedValue <= 0) { OnBelowZero?.Invoke(this,_lastCalculatedValue); }
        }
    }

    public Stat(string name, StatTag tag, float baseValue, bool alarmBelowZero, bool limitValueGreaterBase, Func<float> baseValueFormula = null)
    {
        Name = name;
        Tag = tag;
        _dynamicBaseValueFormula = baseValueFormula;
        if (_dynamicBaseValueFormula != null)
            _initialBaseValue = _dynamicBaseValueFormula();
        else
            _initialBaseValue = baseValue;

        _baseValue = baseValue;  // для безопасности
        _alarmIfBelowZero = alarmBelowZero;
        LimitValueGreaterBase = limitValueGreaterBase;

        _isDirty = true;
    }

    public void AddModifier(StatModifier modifier)
    {
        //Debug.LogWarning($"{this.Name} stat - AddModifier run");
        _permanentModifiers.Add(modifier);
        //Debug.LogWarning($"{this.Name} stat - _permanentModifiers: {_permanentModifiers.Count}");
        _isDirty = true;
        CheckForInvoke();
        //Debug.LogWarning($"{this.Name} stat - NewValue: {Value}");
    }

    public void AddModifier(IStatModifier modifier)
    {
        //Debug.LogWarning($"{this.Name} stat - AddModifier run");
        _permanentModifiers.Add(modifier);
        //Debug.LogWarning($"{this.Name} stat - _permanentModifiers: {_permanentModifiers.Count}");
        _isDirty = true;
        CheckForInvoke();
        Debug.Log($"{this.Name} stat added modifier from {modifier.Source} new Value: {Value}");
    }

    public void AddTimedModifier(ITimedStatModifier modifier)
    {
        _timedModifiers.Add(modifier);
        _isDirty = true;
        CheckForInvoke();
    }

    public bool RemoveModifier(string id)
    {
        int removed = _permanentModifiers.RemoveAll(m => m.Id == id);
        removed += _timedModifiers.RemoveAll(m => m.Id == id);

        if (removed > 0)
        {
            _isDirty = true;
            CheckForInvoke();
            return true;
        }
        return false;
    }

    public bool RemoveModifiersFromSource(string source)
    {
        int removed = _permanentModifiers.RemoveAll(m => m.Source == source);
        removed += _timedModifiers.RemoveAll(m => m.Source == source);

        if (removed > 0)
        {
            _isDirty = true;
            CheckForInvoke();
            return true;
        }
        return false;
    }

    public void UpdateTimedModifiers(float deltaTime)
    {
        bool changed = false;

        for (int i = _timedModifiers.Count - 1; i >= 0; i--)
        {
            var mod = _timedModifiers[i];
            mod.Update(deltaTime);

            if (mod.IsExpired)
            {
                _timedModifiers.RemoveAt(i);
                changed = true;
            }
        }

        if (changed)
        {
            _isDirty = true;
            CheckForInvoke();
        }
    }

    public void SetBaseValue(float newBaseValue)
    {
        if (BaseValue != newBaseValue)
        {
            BaseValue = newBaseValue;
            _isDirty = true;
        }
    }

    //TODO:   REMOVE after tests
    public void TESTSSetValue(float newValue)
    {
        var oldValue = _lastCalculatedValue;
        _lastCalculatedValue = newValue;

        OnValueChanged?.Invoke(this, oldValue, _lastCalculatedValue);
    }

    private const string TmpModifierId = "tmp"; // for AddToTmpModifier method

    public void AddToTmpModifier(float tmpValue)
    {
        // Ищем TMP-модификатор
        var tmpModifier = _permanentModifiers.Find(m => m.Id == TmpModifierId);

        if (tmpModifier != null)
        {
            // Обновляем значение модификатора
            if (tmpModifier is StatModifier sm)
            {
                sm.AddToValue(tmpValue);
            }
            else
            {
                Debug.LogWarning("TMP modifier found but is not StatModifier");
            }
        }
        else
        {
            // Создаем новый TMP модификатор и добавляем
            var newTmp = new StatModifier(TmpModifierId, "default", Tag, tmpValue, StatModType.Flat);
            AddModifier(newTmp);
        }

        _isDirty = true;
        CheckForInvoke();
    }

    private float CalculateFinalValue()
    {
        float finalValue = _initialBaseValue;  // используем фиксированное значение из инициализации
        float sumPercentAdd = 0;
        float productPercentMult = 1f;

        ProcessModifiers(_permanentModifiers, ref finalValue, ref sumPercentAdd, ref productPercentMult);
        ProcessModifiers(_timedModifiers, ref finalValue, ref sumPercentAdd, ref productPercentMult);

        finalValue *= 1 + sumPercentAdd / 100f;
        finalValue *= productPercentMult;

        if (LimitValueGreaterBase && finalValue > BaseValue)
        {
            finalValue = BaseValue;
        }

        return finalValue;
    }

    private void ProcessModifiers<T>(List<T> modifiers, ref float finalValue,
                               ref float sumPercentAdd, ref float productPercentMult) where T : IStatModifier
    {
        foreach (var mod in modifiers)
        {
            // Пропускаем просроченные временные модификаторы
            if (mod is ITimedStatModifier timedMod && timedMod.IsExpired)
                continue;

            switch (mod.Type)
            {
                case StatModType.Flat:
                    finalValue += mod.Value;
                    break;
                case StatModType.PercentAdd:
                    sumPercentAdd += mod.Value;
                    break;
                case StatModType.PercentMult:
                    productPercentMult *= 1 + mod.Value / 100f;
                    break;
            }
        }
    }

    public float GetValuePercentage()
    {
        if (BaseValue == 0f)
            return 0f; // или возвращать 100f, если BaseValue = 0 трактовать как полный объем

        return (Value / BaseValue) * 100f;
    }

    public void CheckAllPermanentModifiers()
    {
        int counter = 0;
        //TEST purpose
        foreach(var item in _permanentModifiers)
        {
            Debug.Log($" {item.TargetStat}: ID = {item.Id} Source: {item.Source} value {item.Value} type{item.Type}");
            counter++;
        }
        Debug.Log($" found {counter} modifiers");
    }

    public int ModifiersCount()
    {
        return _permanentModifiers.Count;
    }

    public override string ToString()
    {
        return $"{Name} ({Tag}): {Value}/{BaseValue} [Initial: {_initialBaseValue}, " +
            $"Mods: {_permanentModifiers.Count + _timedModifiers.Count}, Limit: {LimitValueGreaterBase}]";
    }

    public string GetDebugInfo()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"=== Stat: {Name} ===");
        sb.AppendLine($"Tag: {Tag}");
        sb.AppendLine($"Values: Current={Value:F2}, Base={BaseValue:F2}, Initial={_initialBaseValue:F2}");
        sb.AppendLine($"Flags: LimitGreaterBase={LimitValueGreaterBase}, AlarmBelowZero={_alarmIfBelowZero}, IsDirty={_isDirty}");
        sb.AppendLine($"Dynamic Formula: {(_dynamicBaseValueFormula != null ? "Yes" : "No")}");

        sb.AppendLine($"Permanent Modifiers ({_permanentModifiers.Count}):");
        if (_permanentModifiers.Count > 0)
        {
            foreach (var mod in _permanentModifiers)
                sb.AppendLine($"  - {mod.Source} ({mod.Id}): {mod.Value:F2} ({mod.Type})");
        }
        else
        {
            sb.AppendLine("  None");
        }

        sb.AppendLine($"Timed Modifiers ({_timedModifiers.Count}):");
        if (_timedModifiers.Count > 0)
        {
            foreach (var mod in _timedModifiers)
            {
                string expired = mod.IsExpired ? " [EXPIRED]" : "";
                sb.AppendLine($"  - {mod.Source} ({mod.Id}): {mod.Value:F2} ({mod.Type}){expired}");
            }
        }
        else
        {
            sb.AppendLine("  None");
        }

        return sb.ToString();
    }
}