using Mono.Cecil;

public class StatModifier
{
    public string Id { get; private set; }
    public string Source { get; private set; }
    public StatTag TargetStat { get; private set; }
    public float Value { get; private set; }
    public StatModType Type { get; private set; }

    public StatModifier(string id, string source, StatTag targetStat,
                       float value, StatModType type)
    {
        Id = id;
        Source = source;
        TargetStat = targetStat;
        Value = value;
        Type = type;
    }

    public void AddToValue (float tmpValue)
    {
        Value += tmpValue;
    }
}