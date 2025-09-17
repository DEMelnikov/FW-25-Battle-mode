using UnityEngine;
using UnityEngine.AI;

public interface ICharacter
{
    Transform transform { get; }
    string name { get; }
    ICharacterTargetsVault GetTargetsVault();
    IStatsController GetStatsController();
    GameObject GetSelectedTarget();
    public virtual void SetSelectedTarget(GameObject target) { }
    public SceneObjectTag SceneObjectTag { get; }
    NavMeshAgent GetNavMeshAgent();
    IBehaviorProfile GetBehaviorProfile();
}

public interface IBehaviorProfile
{
    float PursuitDistance { get; }
    float WeaponRange {  get; }
}