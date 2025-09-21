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
    private bool _isSubscribed = false;


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

        // Подписываемся на событие паузы
        SubscribeToPauseEvents();

    }

    public override void OnExit(IStateMachine machine)
    {
        _navMeshAgent.isStopped = true;

        // Отписываемся от события при выходе из состояния
        UnsubscribeFromPauseEvents();

        base.OnExit(machine);
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        if (PauseManager.IsPaused) return;
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {
        if (PauseManager.IsPaused) return;

        base.OnUpdate(machine);
        if (!_targetsVault.TryGetTargetEnemyTransform(out Transform _newTargetTransform))
        {
            if (logging) Debug.Log($"{this.name} - can't get enemy Transform");
            machine.SetState(_whenLooseTargetState);
        }

        if (_targetTransform != _newTargetTransform) _navMeshAgent.SetDestination(_newTargetTransform.position);
        _targetsVault.UpdateDistanceTargetEnemy();
    }

    public void CheckTransitions(IStateMachine machine)
    {
        base.CheckTransitions(machine);
    }

    // Метод для подписки на события паузы
    private void SubscribeToPauseEvents()
    {
        if (!_isSubscribed)
        {
            PauseManager.OnPauseStateChanged += HandlePauseStateChanged;
            _isSubscribed = true;
        }
    }

    // Метод для отписки от событий паузы
    public  void UnsubscribeFromPauseEvents()
    {
        if (_isSubscribed)
        {
            PauseManager.OnPauseStateChanged -= HandlePauseStateChanged;
            _isSubscribed = false;
        }
    }

    // Обработчик изменения состояния паузы
    private void HandlePauseStateChanged(bool isPaused)
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.isStopped = isPaused;
        }
    }
}
