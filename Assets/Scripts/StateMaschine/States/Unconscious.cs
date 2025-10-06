
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
//using static Codice.Client.Common.WebApi.WebApiEndpoints;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Death")]
public class Unconscious : State
{
    //[SerializeField] private State _whenLooseTargetState;
    //[SerializeField] private AbilityCost _energyCost;

    //private ICharacterTargetsVault _targetsVault;
    //private IStatsController _statsController;
    //private NavMeshAgent _navMeshAgent;
    //private IBehaviorProfile _behaviorProfile;
    //private Transform _targetTransform;
    private bool _isSubscribed = false;
    //[SerializeField] private Vector3 _currentTargetCoordinates;

    //private TimerTrigger energyLoseTrigger;



    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);

        //owner = machine.Context.Owner.GetComponent<Character>();
        //owner = machine.Character;
        machine.CharacterGoal = CharacterGlobalState.UNCONSCIOUS;
        owner.TargetsVault.CleanTargetEnemyCharacter();


        SetAnimation();


    }

    public override void OnExit(StateMachine machine)
    {
        //_navMeshAgent.isStopped = true;

        //// Отписываемся от события при выходе из состояния
        //UnsubscribeFromPauseEvents();

        //energyLoseTrigger.UnsubscribeFromPauseEvents();
        //SetAnimation();

        base.OnExit(machine);
    }

    public override void OnFixedUpdate(StateMachine machine)
    {
        if (PauseManager.IsPaused) return;
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(StateMachine machine)
    {
        if (PauseManager.IsPaused) return;

        base.OnUpdate(machine);

        //energyLoseTrigger.Update(Time.deltaTime);

        //if (!_targetsVault.TryGetTargetEnemyTransform(out Transform _newTargetTransform))
        //{
        //    if (logging) Debug.Log($"{this.name} - can't get enemy Transform");
        //    machine.SetState(_whenLooseTargetState);
        //}

        //if (_targetTransform != _newTargetTransform) _navMeshAgent.SetDestination(_newTargetTransform.position);
        //_targetsVault.UpdateDistances();


        //if (_targetsVault.GetCoordinates() == Vector3.zero)
        //{
        //    machine.SetInitialState();
        //    return;
        //}

        //if (_currentTargetCoordinates != _targetsVault.GetCoordinates())
        //{
        //    _currentTargetCoordinates = _targetsVault.GetCoordinates();
        //    _navMeshAgent.SetDestination(_currentTargetCoordinates);
        //}

        ////_targetsVault.UpdateDistanceTargetEnemy();

        //// ПРОВЕРКА ДОСТИЖЕНИЯ ЦЕЛИ
        //if (HasReachedDestination())
        //{
        //    if (logging) Debug.Log($" {owner.gameObject.name}.{this.name} HasReachedDestination");
        //    // Переход в следующее состояние (например, атака)
        //    //Debug.LogWarning("Reached destination");
        //    machine.CharacterGoal = CharacterGlobalGoal.Idle;
        //    machine.SetInitialState(); // TODO или другое целевое состояние
        //    _targetsVault.ClearWayPoint();
        //    _targetsVault.UpdateDistances();
        //    return;
        //}

        //i
    }

    public void CheckTransitions(StateMachine machine)
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
    public void UnsubscribeFromPauseEvents()
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
        //if (_navMeshAgent != null)
        //{
        //    _navMeshAgent.isStopped = isPaused;
        //}
        SetAnimation();
    }

    private void OnTimerTick()
    {
        //if (_navMeshAgent.isStopped) return;

        ////_navMeshAgent.Pa
        ////Debug.Log("Spend energy");
        //_energyCost.PayAbilityCost(owner);
    }

    private bool HasReachedDestination()
    {
        return false;
        //return !_navMeshAgent.pathPending &&
        //       _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
        //       _navMeshAgent.velocity.sqrMagnitude == 0f;
    }

    private void SetAnimation()
    {
        //Debug.LogError($" TRY to SET ANIMATION!");
        //owner.SetAnimationBool(AnimationBools.ISDEAD, true);
        owner.ActivateAnimationTrigger(AnimationTriggers.DEATH);

        //owner.SetAnimationBool(AnimationBools.ISWALK, !_navMeshAgent.isStopped);
    }

}
