using NUnit.Framework;
using System;

public class Stat_AdvancedTests
{
    private const float TOL = 1e-4f;

    private Stat MakeStat(float baseValue = 100f, bool alarmBelowZero = true)
    {
        return new Stat("TestStat", (StatTag)0, baseValue, alarmBelowZero);
    }

    [TestCase(100f, 10f, 0f, 0f, 110f)]
    [TestCase(100f, 0f, 50f, 0f, 150f)]
    [TestCase(100f, 0f, 0f, 10f, 110f)]
    [TestCase(100f, 10f, 50f, 10f, 181.5f)] // (100+10)*1.5*1.1 = 181.5
    public void CombinedModifiers_Work_AsDocumented(float baseV, float flat, float addPct, float multPct, float expected)
    {
        var stat = MakeStat(baseV);

        if (flat != 0f)
            stat.AddModifier(new StatModifier("flat", "src", (StatTag)0, flat, StatModType.Flat));
        if (addPct != 0f)
            stat.AddModifier(new StatModifier("add", "src", (StatTag)0, addPct, StatModType.PercentAdd));
        if (multPct != 0f)
            stat.AddModifier(new StatModifier("mult", "src", (StatTag)0, multPct, StatModType.PercentMult));

        Assert.AreEqual(expected, stat.Value, TOL);
    }

    [Test]
    public void RemoveModifiersFromSource_NoChanges_DoesNotFireEvents()
    {
        var stat = MakeStat(100f);
        int changed = 0, below = 0;
        stat.OnValueChanged += (_, __, ___) => changed++;
        stat.OnBelowZero += (_, __) => below++;

        bool removed = stat.RemoveModifiersFromSource("no-such-source");

        Assert.IsFalse(removed);
        Assert.AreEqual(0, changed);
        Assert.AreEqual(0, below);
        Assert.AreEqual(100f, stat.Value, TOL);
    }

    [Test]
    public void OnBelowZero_Fires_Once_Per_Dirty_Recalc()
    {
        var stat = MakeStat(5f, alarmBelowZero: true);
        int belowCalls = 0;
        stat.OnBelowZero += (_, __) => belowCalls++;

        stat.AddModifier(new StatModifier("neg", "src", (StatTag)0, -10f, StatModType.Flat));
        Assert.AreEqual(-5f, stat.Value, TOL);
        Assert.AreEqual(1, belowCalls, "Должен сработать при уходе в неположительное значение");

        // Повторное чтение без изменений не создаёт новый вызов
        float v2 = stat.Value;
        Assert.AreEqual(-5f, v2, TOL);
        Assert.AreEqual(1, belowCalls, "Не должно срабатывать, если не dirty");

        // Делаем объект dirty — меняем BaseValue так, что он остаётся <= 0 — событие сработает снова
        stat.SetBaseValue(-1f);
        float v3 = stat.Value; // триггер пересчёта
        Assert.LessOrEqual(v3, 0f);
        Assert.AreEqual(2, belowCalls, "Должно срабатывать на каждый новый пересчёт, когда dirty и значение <= 0");
    }

    [Test]
    public void ZeroEffect_Modifiers_DoNotFire_OnValueChanged()
    {
        var stat = MakeStat(100f);
        int changed = 0;
        stat.OnValueChanged += (_, __, ___) => changed++;

        stat.AddModifier(new StatModifier("z1", "src", (StatTag)0, 0f, StatModType.Flat));
        stat.AddModifier(new StatModifier("z2", "src", (StatTag)0, 0f, StatModType.PercentAdd));
        stat.AddModifier(new StatModifier("z3", "src", (StatTag)0, 0f, StatModType.PercentMult));

        Assert.AreEqual(100f, stat.Value, TOL);
        Assert.AreEqual(0, changed);
    }

    [Test]
    public void NegativePercentAdd_To_Zero_Triggers_OnBelowZero()
    {
        var stat = MakeStat(10f, alarmBelowZero: true);
        int below = 0;
        stat.OnBelowZero += (_, __) => below++;

        // -100% от текущего => 0
        stat.AddModifier(new StatModifier("negAdd", "src", (StatTag)0, -100f, StatModType.PercentAdd));
        Assert.AreEqual(0f, stat.Value, TOL);
        Assert.AreEqual(1, below);
    }
}