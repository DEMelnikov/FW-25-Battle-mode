using UnityEngine;

public interface ICharacter
{
    Transform transform { get; }
    string name { get; }
    ICharacterTargetsVault GetTargetsVault();
    IStatsController GetStatsController();
    GameObject GetSelectedTarget();
    public virtual void SetSelectedTarget(GameObject target) { }
    public SceneObjectTag SceneObjectTag { get; }
}
