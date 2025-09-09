using NUnit.Framework;

public class Stat_TimedModifiersTests
{
    private const float TOL = 1e-4f;

    private Stat MakeStat(float baseValue = 100f, bool alarmBelowZero = true)
    {
        return new Stat("TimedStat", (StatTag)0, baseValue, alarmBelowZero);
    }

    [Test]
    public void Timed_Flat_Adds_And_Expires_As_Time_Passes()
    {
        var stat = MakeStat(100f);
        int changed = 0;
        stat.OnValueChanged += (_, __, ___) => changed++;

        // Timed +10 на 1 секунду
        var timed = new TimedStatModifier("t1", "src", (StatTag)0, 10f, StatModType.Flat, 1.0f);
        stat.AddTimedModifier(timed);

        Assert.AreEqual(110f, stat.Value, TOL);

        stat.UpdateTimedModifiers(0.5f);
        Assert.AreEqual(110f, stat.Value, TOL, "Ещё не истёк");

        stat.UpdateTimedModifiers(0.6f);
        Assert.AreEqual(100f, stat.Value, TOL, "Истёк и удалён");

        // События: при добавлении и при истечении
        Assert.GreaterOrEqual(changed, 2);
    }

    [Test]
    public void Multiple_Timed_Modifiers_Expire_Independently()
    {
        var stat = MakeStat(100f);

        var t1 = new TimedStatModifier("t1", "src", (StatTag)0, 5f, StatModType.Flat, 0.5f);
        var t2 = new TimedStatModifier("t2", "src", (StatTag)0, 10f, StatModType.Flat, 1.0f);

        stat.AddTimedModifier(t1); // 105
        stat.AddTimedModifier(t2); // 115
        Assert.AreEqual(115f, stat.Value, TOL);

        stat.UpdateTimedModifiers(0.6f);    // t1 истёк
        Assert.AreEqual(110f, stat.Value, TOL);

        stat.UpdateTimedModifiers(0.5f);    // t2 истёк
        Assert.AreEqual(100f, stat.Value, TOL);
    }

    [Test]
    public void NoEvents_While_NoTimedModifier_Expired()
    {
        var stat = MakeStat(100f);
        int changed = 0, below = 0;
        stat.OnValueChanged += (_, __, ___) => changed++;
        stat.OnBelowZero += (_, __) => below++;

        var t = new TimedStatModifier("t", "src", (StatTag)0, 10f, StatModType.Flat, 2.0f);
        stat.AddTimedModifier(t);
        Assert.AreEqual(110f, stat.Value, TOL);

        // Идёт время, но ещё не истёк — никаких событий (UpdateTimedModifiers сам по себе не делает dirty)
        stat.UpdateTimedModifiers(0.5f);
        Assert.AreEqual(110f, stat.Value, TOL);
        Assert.AreEqual(0, below);
        // changed уже инкрементировался при добавлении, после апдейта не должен увеличиться
        int changedAfterAdd = changed;

        stat.UpdateTimedModifiers(0.4f);
        Assert.AreEqual(110f, stat.Value, TOL);
        Assert.AreEqual(changedAfterAdd, changed);
    }

    [Test]
    public void Timed_Negative_Modifier_Triggers_OnBelowZero_Then_Recovers_On_Expire()
    {
        var stat = MakeStat(10f, alarmBelowZero: true);
        int below = 0;
        stat.OnBelowZero += (_, __) => below++;

        var t = new TimedStatModifier("neg", "src", (StatTag)0, -20f, StatModType.Flat, 0.1f);
        stat.AddTimedModifier(t);

        Assert.LessOrEqual(stat.Value, 0f);
        Assert.AreEqual(1, below, "Сработал при уходе в 0/ниже");

        stat.UpdateTimedModifiers(0.2f); // истечение
        Assert.Greater(stat.Value, 0f);
        Assert.AreEqual(1, below, "При восстановлении выше нуля событие не должно повторяться");
    }

    [Test]
    public void Remove_Timed_Modifier_ById_Updates_Value_And_ReturnsTrue()
    {
        var stat = MakeStat(100f);
        stat.AddTimedModifier(new TimedStatModifier("tid", "src", (StatTag)0, 25f, StatModType.Flat, 10f));
        Assert.AreEqual(125f, stat.Value, TOL);

        bool removed = stat.RemoveModifier("tid");
        Assert.IsTrue(removed);
        Assert.AreEqual(100f, stat.Value, TOL);
    }

    [Test]
    public void Timed_PercentAdd_And_PercentMult_Affect_Value_And_Expire()
    {
        var stat = MakeStat(100f);

        var add = new TimedStatModifier("add", "src", (StatTag)0, 50f, StatModType.PercentAdd, 1.0f);  // +50% => *1.5
        var mul = new TimedStatModifier("mul", "src", (StatTag)0, 10f, StatModType.PercentMult, 1.0f); // +10% => *1.1

        stat.AddTimedModifier(add);
        stat.AddTimedModifier(mul);

        // 100 * 1.5 * 1.1 = 165
        Assert.AreEqual(165f, stat.Value, TOL);

        stat.UpdateTimedModifiers(1.1f); // оба истекают
        Assert.AreEqual(100f, stat.Value, TOL);
    }

    [Test]
    public void Adding_Expired_Timed_Modifier_Applies_Until_First_Update_Then_Removed()
    {
        var stat = MakeStat(100f);

        // Duration = 0 => IsExpired сразу true, но код Stat удаляет только при UpdateTimedModifiers
        var expired = new TimedStatModifier("e", "src", (StatTag)0, 10f, StatModType.Flat, 0f);
        stat.AddTimedModifier(expired);

        // Текущее поведение: значение учитывается до первого апдейта
        Assert.AreEqual(110f, stat.Value, TOL);

        stat.UpdateTimedModifiers(0f); // триггерим очистку
        Assert.AreEqual(100f, stat.Value, TOL);
    }
}