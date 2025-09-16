using UnityEngine;

[CreateAssetMenu(fileName = "PursuitDistanceTrigger", menuName = "FW25/Triggers/PursuitDistanceTrigger")]
public class PursuitDistanceTrigger : Trigger
{

    [SerializeField] private RelationType _relationType = RelationType.Lesser;
    private float _actualDistance;
    private float _targetDistance;
    [SerializeField] private float _equalityTolerance = 0.5f; // Допуск для сравнения на равенство

    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log("Check trigger PursuitDistanceTrigger started");

        if (!character.GetTargetsVault().TryGetTargetEnemy(out GameObject enemy)) return false;
        _targetDistance = character.GetBehaviorProfile().PursuitDistance;
        if(_targetDistance < 0) return false;

        Vector3 enemyPosition = enemy.transform.position;
        Vector3 heroPosition = character.transform.position;
        
        // Вычисляем фактическое расстояние
        _actualDistance = Vector3.Distance(heroPosition, enemyPosition);


        if (logging) Debug.Log($"Check trigger PursuitDistanceTrigger:_actualDistance = {_actualDistance}");

        // Сравниваем с целевым расстоянием
        return CompareDistancesWithTolerance(_actualDistance, _targetDistance, _relationType, _equalityTolerance);
    }

    private bool CompareDistancesWithTolerance(float actualDistance, float targetDistance, RelationType relation, float tolerance)
    {
        switch (relation)
        {
            case RelationType.Lesser:
                return actualDistance < targetDistance;

            case RelationType.Equal:
                return Mathf.Abs(actualDistance - targetDistance) <= tolerance;

            case RelationType.Greater:
                return actualDistance > targetDistance;

            default:
                Debug.LogWarning($"Unknown RelationType: {relation}");
                return false;
        }
    }
}
