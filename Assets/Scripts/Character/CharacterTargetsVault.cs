using UnityEditorInternal;
using UnityEngine;
using EditorAttributes;

[DisallowMultipleComponent]
public class CharacterTargetsVault : MonoBehaviour, ICharacterTargetsVault
{
    [SerializeField]  private GameObject _selectedTargetGO; //For Inspector purpose only
    [SerializeField] private ICharacter _targetEnemyChararacter;

    [SerializeField] private Vector3 _waypoint = Vector3.zero;
    [SerializeField] private SceneObjectTag _whoIsYourEnemy = SceneObjectTag.Enemy;
    [SerializeField] protected bool logging = true;
    [SerializeField] private float enemyActualDistance;
    [SerializeField] private float wpActualDistance;
                     private IStateMachine _sm;
                     private BehaviorProfile _behaviorProfile;

    public event System.Action<ICharacter> OnTargetEnemySet;        // Когда установлена новая цель
    public event System.Action<ICharacter> OnTargetEnemyCleared;    // Когда цель очищена

    #region UnityMethods and Initialize
    void Awake()
    {

    }

    private void OnDestroy()
    {
        UnSubscribetToTargetsFSM();
        ClearAllSubscribers();
    }

    public void Initialize()
    {
        _sm = GetComponent<Character>().StateMachine;
        _behaviorProfile = GetComponent<BehaviorProfile>();
    }

    // Очистка всех подписчиков событий
    public void ClearAllSubscribers()
    {
        OnTargetEnemySet = null;
        OnTargetEnemyCleared = null;
    }

    #endregion

    #region Subscribe/Unsubscribe methods
    private void SubscribetToTargetsFSM()
    {
        if (!TryGetTargetCharacter(out var _targetChararacter)) return;

        _targetChararacter.StateMachine.OnStateChangedFSM += OnTargetsFSMStateChanged;
        if (logging) Debug.Log($"{this.gameObject} - Subscribed to FSM Changes of {_targetChararacter.ObjName}");
    }

    private void UnSubscribetToTargetsFSM()
    {
        if (!TryGetTargetCharacter(out var _targetChararacter)) return;

        _targetChararacter.StateMachine.OnStateChangedFSM -= OnTargetsFSMStateChanged;
        if (logging) Debug.Log($"{this.gameObject} - UnSubscribed to FSM Changes of {_targetChararacter.ObjName}");
    }

    public void OnTargetsFSMStateChanged(FSMStates oldState, FSMStates newState)
    {
        Debug.LogWarning($"{this.gameObject.name} - oppoenet changed State to {newState}");
    }

    #endregion

    #region Target Enemy methods

    public bool TryGetTargetCharacter(out ICharacter targetICharacter)
    {
        targetICharacter = null;
        if (!ValidateTargetEnemy()) return false;


        if (_targetEnemyChararacter != null)
        {
            targetICharacter = _targetEnemyChararacter;

            //Debug.LogWarning($"I'm inside TryGetComponent targetICharacter name {targetICharacter.GetGameObject.name} ");

            return targetICharacter != null; 
        }

        return false;
    }

    private bool ValidateTargetEnemy()
    {
        if (_targetEnemyChararacter != null &&
            _targetEnemyChararacter.SceneObjectTag == _whoIsYourEnemy)
        {
            return true;
        }
        return false;
    }


    private void UpdateDistanceTargetEnemy()
    {
        enemyActualDistance = Mathf.Infinity;
        ValidateAndCleanTarget();

        if (_targetEnemyChararacter == null) return;
        Transform enemyTransform = _targetEnemyChararacter.transform;

        enemyActualDistance = Vector3.Distance(this.gameObject.transform.position, enemyTransform.position);
    }

    public bool TryGetTargetEnemyTransform (out Transform transform)
    {
        if (TryGetTargetCharacter(out var enemy))
        {
            //if (logging) Debug.Log($"{gameObject.name} CharacterTargets.TryGetTargetEnemy() Got {_enemy.name} and now geting Transform");
            transform = enemy.transform;
            //if (logging) Debug.Log($" Transform position X: {_transform.position.x}");
            return true; 
        }

        transform = null;
        return false;
    }

    // ����������� HasTargetEnemy - ������ ��� �������� ��������!
    public bool HasTargetEnemy()
    {
        return ValidateTargetEnemy();
    }

    public void ValidateAndCleanTarget()
    {
        if (!ValidateTargetEnemy()) { _targetEnemyChararacter = null; }
    }
    
    //public void SetTargetEnemyCharacter(ICharacter target)
    //{
    //    if (target == null) return;

    //    if (target.SceneObjectTag == _whoIsYourEnemy)
    //    {
    //        //Debug.LogWarning("I'm in targetsVault");

    //        _targetEnemyChararacter = target;
    //        if (logging) Debug.Log($"{gameObject.name} Get new Alive Target {_targetEnemyChararacter.ObjName}");

    //        //TODO ������ �����
    //        _selectedTargetGO = target.GetGameObject;
    //        SubscribetToTargetsFSM();
    //    }
    //    else if (logging)
    //    {
    //        Debug.Log($"{gameObject.name} SetTargetEnemyCharacter rejected " +
    //            $"- target tag {target.SceneObjectTag} != enemy tag {_whoIsYourEnemy}");
    //    }

    //}

    public void SetTargetEnemyCharacter(ICharacter target)
    {
        if (target == null) return;

        if (target.SceneObjectTag == _whoIsYourEnemy)
        {
            ICharacter previousTarget = _targetEnemyChararacter;

            // Если это новая цель (не та же самая)
            if (previousTarget != target)
            {
                // Отписываемся от старой цели
                if (previousTarget != null)
                {
                    UnSubscribetToTargetsFSM();
                }

                // Устанавливаем новую цель
                _targetEnemyChararacter = target;
                _selectedTargetGO = target.GetGameObject;

                // Подписываемся на события новой цели
                SubscribetToTargetsFSM();

                // Оповещаем подписчиков
                OnTargetEnemySet?.Invoke(target);

                if (logging) Debug.Log($"{gameObject.name} Get new Alive Target {_targetEnemyChararacter.ObjName}");
            }
        }
        else if (logging)
        {
            Debug.Log($"{gameObject.name} SetTargetEnemyCharacter rejected " +
                $"- target tag {target.SceneObjectTag} != enemy tag {_whoIsYourEnemy}");
        }
    }

    public void CleanTargetEnemyCharacter()
    {
        ICharacter previousTarget = _targetEnemyChararacter;

        if (previousTarget != null)
        {
            // Отписываемся от событий цели
            UnSubscribetToTargetsFSM();

            // Очищаем ссылки
            _targetEnemyChararacter = null;
            _selectedTargetGO = null;

            // Оповещаем подписчиков
            OnTargetEnemyCleared?.Invoke(previousTarget);

            if (logging) Debug.Log($"{gameObject.name} Cleared target {previousTarget.ObjName}");
        }
        else
        {
            _targetEnemyChararacter = null;
            _selectedTargetGO = null;
        }
    }


    #endregion

    #region Waypoint methods

    public Vector3 GetWayPoint()
    {
        return _waypoint;
    }

    private void UpdateDistanceWayPoint()
    {
        wpActualDistance = Mathf.Infinity;
        if (_waypoint == Vector3.zero) return;

        //Ve enemyTransform = _selectedTarget.transform;

        wpActualDistance = Vector3.Distance(this.gameObject.transform.position, _waypoint);
    }

    public void SetWayPoint(Vector3 wp)
    {
        //Debug.LogWarning($"I'M in SetWaypoint {wp.x}");
        _waypoint = wp;
    }

    public void ClearWayPoint()
    {
        _waypoint = Vector3.zero;
        UpdateDistanceWayPoint();
    }
    #endregion

    #region Universal


    public Vector3 GetCoordinates()
    {
        switch (GetCharacterGoal())
        {
            case CharacterGlobalState.MoveToPoint:
                {
                    return GetWayPoint();
                }
            case CharacterGlobalState.Attack:
                {
                    Vector3 coordinates = Vector3.zero;
                    if (TryGetTargetEnemyTransform(out var targetTransform))  coordinates = targetTransform.position; 
                    return coordinates;
                }
            case CharacterGlobalState.Chase:
                {
                    Vector3 coordinates = Vector3.zero;
                    if (TryGetTargetEnemyTransform(out var targetTransform)) coordinates = targetTransform.position;
                    return coordinates;
                }
            default: return Vector3.zero;
        }
    }

    private CharacterGlobalState GetCharacterGoal()
    {
        return _sm.CharacterGoal;
    }
    
    public void UpdateDistances()
    {
        if (_targetEnemyChararacter != null) UpdateDistanceTargetEnemy();
        if (_waypoint != Vector3.zero) UpdateDistanceWayPoint();
    }

    #endregion

    public float ActualEnemyDistance
    {
        get
        {
            UpdateDistanceTargetEnemy();
            return enemyActualDistance;
        }
        set
        {
            enemyActualDistance = value;
        }
    }
}