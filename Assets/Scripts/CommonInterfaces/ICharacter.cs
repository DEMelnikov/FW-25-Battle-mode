using UnityEngine;

public interface ICharacter
{
    Transform transform { get; }
    string name { get; }
    ICharacterTargetsVault GetTargets();
    IStatsController GetStatsController();
    GameObject GetSelectedTarget();
}
