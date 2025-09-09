using NUnit.Framework;
using System;
using System.Collections.Generic;

public class StatTests
{
    private const float TOL = 1e-4f;

    private Stat MakeStat(float baseValue = 100f, bool alarmBelowZero = true)
    {
        // StatTag нам не важен в расчетах Ч используем 0
        return new Stat("TestStat", (StatTag)0, baseValue, alarmBelowZero);
    }

    [Test]
    public void Initial_Value_Equals_BaseValue()
    {
        var stat = MakeStat(123f);
        Assert.AreEqual(123f, stat.Value, TOL);
    }

    [Test]
    public void Add_Flat_Modifier_Changes_Value_And_Fires_OnValueChanged()
    {
        var stat = MakeStat(100f);
        int calls = 0;
        float lastOld = float.NaN, lastNew = float.NaN;
        stat.OnValueChanged += (_, oldV, newV) => { calls++; lastOld = oldV; lastNew = newV; };

        var flat = new StatModifier("flat1", "srcA", (StatTag)0, 15f, StatModType.Flat);
        stat.AddModifier(flat);

        Assert.AreEqual(115f, stat.Value, TOL);
        Assert.AreEqual(1, calls);          // событие сработало
        Assert.AreEqual(100f, lastOld, TOL);
        Assert.AreEqual(115f, lastNew, TOL);
    }

    [Test]
    public void Add_Zero_Flat_Does_Not_Fire_OnValueChanged()
    {
        var stat = MakeStat(100f);
        int calls = 0;
        stat.OnValueChanged += (_, __, ___) => calls++;

        var zero = new StatModifier("zero", "src", (StatTag)0, 0f, StatModType.Flat);
        stat.AddModifier(zero);

        Assert.AreEqual(100f, stat.Value, TOL);
        Assert.AreEqual(0, calls); // значение не помен€лось Ч событи€ нет
    }

    [Test]
    public void PercentAdd_And_PercentMult_Combine_Correctly()
    {
        var stat = MakeStat(100f);
        // 2 аддитивных процента суммируютс€: +20% и +30% => +50% => 100 * 1.5 = 150
        stat.AddModifier(new StatModifier("pa1", "src", (StatTag)0, 20f, StatModType.PercentAdd));
        stat.AddModifier(new StatModifier("pa2", "src", (StatTag)0, 30f, StatModType.PercentAdd));
        Assert.AreEqual(150f, stat.Value, TOL);

        // ћультипликативные множатс€: +10% => *1.1, ещЄ +10% => *1.1 => 150 * 1.21 = 181.5
        stat.AddModifier(new StatModifier("pm1", "src", (StatTag)0, 10f, StatModType.PercentMult));
        stat.AddModifier(new StatModifier("pm2", "src", (StatTag)0, 10f, StatModType.PercentMult));
        Assert.AreEqual(181.5f, stat.Value, TOL);
    }

    [Test]
    public void RemoveModifier_ById_Updates_Value_And_ReturnsTrue()
    {
        var stat = MakeStat(100f);
        stat.AddModifier(new StatModifier("a", "src", (StatTag)0, 10f, StatModType.Flat));   // 110
        stat.AddModifier(new StatModifier("b", "src", (StatTag)0, 20f, StatModType.Flat));   // 130

        bool removed = stat.RemoveModifier("a");
        Assert.IsTrue(removed);
        Assert.AreEqual(120f, stat.Value, TOL);

        // ѕовторное удаление того же id Ч false
        Assert.IsFalse(stat.RemoveModifier("a"));
    }

    [Test]
    public void RemoveModifiers_FromSource_Removes_All_From_That_Source()
    {
        var stat = MakeStat(100f);
        stat.AddModifier(new StatModifier("a1", "X", (StatTag)0, 10f, StatModType.Flat));
        stat.AddModifier(new StatModifier("a2", "X", (StatTag)0, 5f, StatModType.Flat));
        stat.AddModifier(new StatModifier("b1", "Y", (StatTag)0, 3f, StatModType.Flat));

        bool removed = stat.RemoveModifiersFromSource("X");
        Assert.IsTrue(removed);
        Assert.AreEqual(103f, stat.Value, TOL); // осталс€ только Y:+3
    }

    [Test]
    public void AddToTmpModifier_Uses_Default_Tmp_Modifier_And_Fires_Event()
    {
        var stat = MakeStat(100f);
        int calls = 0;
        stat.OnValueChanged += (_, __, ___) => calls++;

        stat.AddToTmpModifier(7f);
        Assert.AreEqual(107f, stat.Value, TOL);
        Assert.AreEqual(1, calls);
    }

    [Test]
    public void SetBaseValue_MarksDirty_But_Fires_OnValueChanged_On_Access()
    {
        var stat = MakeStat(100f);
        int calls = 0;
        float oldV = 0, newV = 0;
        stat.OnValueChanged += (_, old, @new) => { calls++; oldV = old; newV = @new; };

        stat.SetBaseValue(150f);
        Assert.AreEqual(0, calls, "—обытие не должно сработать до пересчЄта");

        // “ригерим пересчЄт чтением Value
        float v = stat.Value;
        Assert.AreEqual(150f, v, TOL);
        Assert.AreEqual(1, calls);
        Assert.AreEqual(100f, oldV, TOL);
        Assert.AreEqual(150f, newV, TOL);
    }

    [Test]
    public void OnBelowZero_Fires_When_Value_Becomes_NonPositive()
    {
        var stat = MakeStat(1f, alarmBelowZero: true);

        int belowCalls = 0;
        float lastBelowValue = float.NaN;
        stat.OnBelowZero += (_, val) => { belowCalls++; lastBelowValue = val; };

        // ”водим в минус
        stat.AddModifier(new StatModifier("neg", "src", (StatTag)0, -5f, StatModType.Flat));

        Assert.AreEqual(-4f, stat.Value, TOL);
        Assert.AreEqual(1, belowCalls);
        Assert.AreEqual(-4f, lastBelowValue, TOL);
    }

    [Test]
    public void OnBelowZero_Does_Not_Fire_When_Alarm_Disabled()
    {
        var stat = MakeStat(1f, alarmBelowZero: false);

        int belowCalls = 0;
        stat.OnBelowZero += (_, __) => belowCalls++;

        stat.AddModifier(new StatModifier("neg", "src", (StatTag)0, -5f, StatModType.Flat));
        Assert.AreEqual(-4f, stat.Value, TOL);
        Assert.AreEqual(0, belowCalls);
    }

    [Test]
    public void Repeated_Value_Reads_Do_Not_Fire_OnValueChanged_When_Not_Dirty()
    {
        var stat = MakeStat(100f);
        int calls = 0;
        stat.OnValueChanged += (_, __, ___) => calls++;

        // ”же пересчитано; повторные чтени€ не должны трогать событие
        float v1 = stat.Value;
        float v2 = stat.Value;
        Assert.AreEqual(100f, v1, TOL);
        Assert.AreEqual(100f, v2, TOL);
        Assert.AreEqual(0, calls);
    }
}