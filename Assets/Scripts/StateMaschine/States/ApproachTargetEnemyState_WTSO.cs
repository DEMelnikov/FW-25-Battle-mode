using UnityEngine;
using UnityEngine.AI;
//using static Codice.Client.Common.WebApi.WebApiEndpoints;

[CreateAssetMenu(menuName = "FW25/State Machine/States/ApproachTargetEnemyState_WTSO")]
public class ApproachTargetEnemyState_WTSO : State
{
    [SerializeField] private State _whenLooseTargetState;
    [SerializeField] private AbilityCost _energyCost;

    private ICharacterTargetsVault _targetsVault;
    private IStatsController _statsController;
    private NavMeshAgent _navMeshAgent;
    private IBehaviorProfile _behaviorProfile;
    private Transform _targetTransform;
    private bool _isSubscribed = false;

    private TimerTrigger energyLoseTrigger;

    private Character owner;

    public override void OnEnter(IStateMachine machine)
    {
        base.OnEnter(machine);

        owner = machine.Context.Owner.GetComponent<Character>();
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

        _navMeshAgent.speed = 2f;          // �������� ��������
        _navMeshAgent.angularSpeed = 120f; // �������� ��������
        _navMeshAgent.acceleration = 8f;   // ���������
        _navMeshAgent.stoppingDistance = 1f; // ��������� ���������

        // ������������� �� ������� �����
        SubscribeToPauseEvents();

        energyLoseTrigger = new TimerTrigger(
            duration: 1f, // ��������, 1 �������
            onTick: () => OnTimerTick(),
            onStart: null,
            onComplete: null,
            looped: true);

        energyLoseTrigger.Start();
        if (_energyCost == null) Debug.LogError($"{owner.gameObject.name}.{this.name} has no ABilityCost" );
    }

    public override void OnExit(IStateMachine machine)
    {
        _navMeshAgent.isStopped = true;

        // ������������ �� ������� ��� ������ �� ���������
        UnsubscribeFromPauseEvents();

        energyLoseTrigger.UnsubscribeFromPauseEvents();
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

        energyLoseTrigger.Update(Time.deltaTime);

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

    // ����� ��� �������� �� ������� �����
    private void SubscribeToPauseEvents()
    {
        if (!_isSubscribed)
        {
            PauseManager.OnPauseStateChanged += HandlePauseStateChanged;
            _isSubscribed = true;
        }
    }

    // ����� ��� ������� �� ������� �����
    public  void UnsubscribeFromPauseEvents()
    {
        if (_isSubscribed)
        {
            PauseManager.OnPauseStateChanged -= HandlePauseStateChanged;
            _isSubscribed = false;
        }
    }

    // ���������� ��������� ��������� �����
    private void HandlePauseStateChanged(bool isPaused)
    {
        if (_navMeshAgent != null)
        {
            _navMeshAgent.isStopped = isPaused;
        }
    }

    private void OnTimerTick()
    {
        if (_navMeshAgent.isStopped) return;

        //Debug.Log("Spend energy");
        _energyCost.PayAbilityCost(owner);
    }
}
