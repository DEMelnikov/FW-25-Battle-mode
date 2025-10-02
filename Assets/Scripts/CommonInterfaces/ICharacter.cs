using UnityEngine;
using UnityEngine.AI;

public interface ICharacter
{
    Transform transform { get; }
    GameObject GetGameObject { get; }
    string ObjName { get; }
    ICharacterTargetsVault TargetsVault { get; }
    IStatsController StatController { get; }
    public virtual void SetSelectedTarget(GameObject target) { }
    public SceneObjectTag SceneObjectTag { get; }
    NavMeshAgent GetNavMeshAgent();
    IBehaviorProfile GetBehaviorProfile();
    IStateMachine GetStateMachine();
    bool InEngage {  get; }
    void UnderMeleAttack(GameObject agressor);
    void ActivateAnimationTrigger(AnimationTriggers trigger);
    void SetAnimationBool(AnimationBools stateName, bool _bool);

}

