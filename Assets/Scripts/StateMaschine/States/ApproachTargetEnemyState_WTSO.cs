using UnityEngine;
using UnityEngine.AI;
//using static Codice.Client.Common.WebApi.WebApiEndpoints;

[CreateAssetMenu(menuName = "FW25/State Machine/States/ApproachTargetEnemyState_WTSO")]
public class ApproachTargetEnemyState_WTSO : State
{
    [SerializeField] private State _whenLooseTargetState;
    private ICharacterTargetsVault _targetsVault;
    private IStatsController _statsController;
    private NavMeshAgent _navMeshAgent;
    private IBehaviorProfile _behaviorProfile;
    private Transform _targetTransform;
    //private float _pauseSaveSpeed;


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
            if (logging) Debug.Log($"{this.name} - can't get enemy Transform");
            machine.SetState(_whenLooseTargetState);
        }

        _navMeshAgent.SetDestination(_targetTransform.position);

        _navMeshAgent.speed = 2f;          // Скорость движения
        _navMeshAgent.angularSpeed = 120f; // Скорость поворота
        _navMeshAgent.acceleration = 8f;   // Ускорение
        _navMeshAgent.stoppingDistance = 1f; // Дистанция остановки

        StopAgentAtPause();

    }

    public override void OnExit(IStateMachine machine)
    {
        base.OnExit(machine);
        _navMeshAgent.isStopped = true;
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        StopAgentAtPause();
        if (PauseManager.IsPaused) return;
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {

        StopAgentAtPause();
        if (PauseManager.IsPaused) return;

        base.OnUpdate(machine);
        if (!_targetsVault.TryGetTargetEnemyTransform(out Transform _newTargetTransform))
        {
            if (logging) Debug.Log($"{this.name} - can't get enemy Transform");
            machine.SetState(_whenLooseTargetState);
        }

        if (_targetTransform != _newTargetTransform) _navMeshAgent.SetDestination(_newTargetTransform.position);
    }

    public void CheckTransitions(IStateMachine machine)
    {
        base.CheckTransitions(machine);
    }

    private void StopAgentAtPause()
    {
        if (PauseManager.IsPaused)
        {
            _navMeshAgent.isStopped = true;
        }         
        else
        {
            _navMeshAgent.isStopped = false;
        }
    }
}
