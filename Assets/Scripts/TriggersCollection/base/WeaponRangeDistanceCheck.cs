using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRangeDistanceCheck", menuName = "FW25/Triggers/WeaponRangeDistanceCheck")]
public class WeaponRangeDistanceCheck : Trigger
{

    [SerializeField] private RelationType _relationType = RelationType.Lesser;
    private float _actualDistance;
    private float _weaponRange;
    [SerializeField] private float _equalityTolerance = 0.5f; // Допуск для сравнения на равенство

    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log($"Check trigger {this.name} started {_relationType} mode");

        if (!character.GetTargetsVault().TryGetTargetEnemy(out GameObject enemy)) return false;
        _weaponRange = character.GetBehaviorProfile().WeaponRange;
        if (_weaponRange < 0) return false;

        Vector3 enemyPosition = enemy.transform.position;
        Vector3 heroPosition = character.transform.position;

        // Вычисляем фактическое расстояние
        _actualDistance = Vector3.Distance(heroPosition, enemyPosition);


        // Сравниваем с целевым расстоянием
        return CompareDistancesWithTolerance(_actualDistance, _weaponRange, _relationType, _equalityTolerance);
    }

    private bool CompareDistancesWithTolerance(float actualDistance, float _weaponRange, RelationType relation, float tolerance)
    {

        switch (relation)
        {
            case RelationType.Lesser:
                if (logging) Debug.Log($"Check trigger PursuitDistanceTrigger:_actualDistance = {actualDistance} _weaponRange= {_weaponRange}");
                return actualDistance < _weaponRange;

            case RelationType.Equal:
                if (logging) Debug.Log($"Check trigger PursuitDistanceTrigger:_actualDistance = {actualDistance} _weaponRange= {_weaponRange}");
                return Mathf.Abs(actualDistance - _weaponRange) <= tolerance;

            case RelationType.Greater:
                if (logging) Debug.Log($"Check trigger PursuitDistanceTrigger:_actualDistance = {actualDistance} _weaponRange= {_weaponRange}");
                return actualDistance > _weaponRange;

            default:
                Debug.LogWarning($"Unknown RelationType: {relation}");
                return false;
        }
    }
}
