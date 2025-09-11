using UnityEngine;

public interface ITimedStatModifier : IStatModifier
{
    //string Id { get; }
    //string Source { get; }
    void Update(float deltaTime);
    bool IsExpired { get; }
    //StatModType Type { get; }
}
