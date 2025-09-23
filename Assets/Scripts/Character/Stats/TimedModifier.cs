using Mono.Cecil;

public class TimedStatModifier : StatModifier,ITimedStatModifier
{
    public float Duration { get; private set; }
    public float TimeRemaining { get; private set; }
    public bool IsExpired => TimeRemaining <= 0;

    public TimedStatModifier(string id, string source, StatTag targetStat,
                            float value, StatModType type, float duration)
        : base(id, source, targetStat, value, type)
    {
        Duration = duration;
        TimeRemaining = duration;
    }

    public void Update(float deltaTime)
    {
        TimeRemaining -= deltaTime;
    }
}