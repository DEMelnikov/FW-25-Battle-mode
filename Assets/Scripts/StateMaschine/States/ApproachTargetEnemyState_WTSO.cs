using UnityEngine;
using UnityEngine.AI;
using static Codice.Client.Common.WebApi.WebApiEndpoints;

[CreateAssetMenu(menuName = "FW25/State Machine/States/ApproachTargetEnemyState_WTSO")]
public class ApproachTargetEnemyState_WTSO : State
{
    [SerializeField] private State _whenLooseTargetState;
    private ICharacterTargetsVault _targetsVault;
    private IStatsController _statsController;
    private NavMeshAgent _navMeshAgent;
    private IBehaviorProfile _behaviorProfile;
    private Transform _targetTransform;


    public override void OnEnter(IStateMachine machine)
    {
        base.OnEnter(machine);

        var owner = machine.Context.Owner.GetComponent<Character>();
        _targetsVault    = owner.GetTargetsVault();
        _statsController = owner.GetStatsController();
        _navMeshAgent    = owner.GetNavMeshAgent();
        _behaviorProfile = owner.GetBehaviorProfile();

        if( !_targetsVault.TryGetTargetEnemyTransform(out _targetTransform))
        {
            machine.SetState(_whenLooseTargetState);
        }

        _navMeshAgent.SetDestination(_targetTransform.position);

        _navMeshAgent.speed = 5f;          // Скорость движения
        _navMeshAgent.angularSpeed = 120f; // Скорость поворота
        _navMeshAgent.acceleration = 8f;   // Ускорение
        _navMeshAgent.stoppingDistance = 1f; // Дистанция остановки


    }

    public override void OnExit(IStateMachine machine)
    {
        base.OnExit(machine);
        _navMeshAgent.speed = 5f;
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {
        base.OnUpdate(machine);
        if (!_targetsVault.TryGetTargetEnemyTransform(out Transform _newTargetTransform))
        {
            machine.SetState(_whenLooseTargetState);
        }

        if (_targetTransform != _newTargetTransform) _navMeshAgent.SetDestination(_newTargetTransform.position);
    }

    public void CheckTransitions(IStateMachine machine)
    {
        base.CheckTransitions(machine);
    }
}
