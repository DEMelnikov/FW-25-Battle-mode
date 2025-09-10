using Mono.Cecil;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using UnityEngine;

public class Stat
{
    public string Name { get; private set; }
    public StatTag Tag { get; private set; }
    public float BaseValue { get; private set; }

    private float _lastCalculatedValue;
    private bool _isDirty = true;
    private bool _alarmIfBelowZero = true;
    private List<StatModifier> _permanentModifiers = new List<StatModifier>();
    private List<TimedStatModifier> _timedModifiers = new List<TimedStatModifier>();

    public event Action<Stat, float, float> OnValueChanged;
    public event Action<Stat, float> OnBelowZero;

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
            return _lastCalculatedValue;
        }
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


    public Stat(string name, StatTag tag, float baseValue, bool alarmBelowZero)
    {
        Name = name;
        Tag = tag;
        BaseValue = baseValue;
        _alarmIfBelowZero = alarmBelowZero;

        this.AddModifier(new StatModifier("tmp","default",tag,0,StatModType.Flat));
    }

    public void AddModifier(StatModifier modifier)
    {
        _permanentModifiers.Add(modifier);
        _isDirty = true;
        CheckForInvoke();
    }

    public void AddTimedModifier(TimedStatModifier modifier)
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

    public void AddToTmpModifier(float tmpValue)
    {
        _permanentModifiers[0].AddToValue(tmpValue);
        _isDirty = true;
        CheckForInvoke() ;
    }

    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;
        float productPercentMult = 1f;

        // Обрабатываем все модификаторы (постоянные и временные)
        ProcessModifiers(_permanentModifiers, ref finalValue, ref sumPercentAdd, ref productPercentMult);
        ProcessModifiers(_timedModifiers, ref finalValue, ref sumPercentAdd, ref productPercentMult);

        // Применяем PercentAdd
        finalValue *= 1 + sumPercentAdd / 100f;

        // Применяем PercentMult
        finalValue *= productPercentMult;

        return finalValue;
    }

    private void ProcessModifiers<T>(List<T> modifiers, ref float finalValue,
                                   ref float sumPercentAdd, ref float productPercentMult) where T : StatModifier
    {
        foreach (var mod in modifiers)
        {
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
}