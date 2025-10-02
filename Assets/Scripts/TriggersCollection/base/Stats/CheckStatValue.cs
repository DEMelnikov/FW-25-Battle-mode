
using UnityEngine;

[CreateAssetMenu(fileName = "CheckStatValue", menuName = "FW25/Triggers/Stats/CheckStatValue")]
public class CheckStatValue : Trigger
{
    [SerializeField] private StatTag _statToCheck = StatTag.ENERGY;
    [SerializeField] private RelationType _relationType = RelationType.Lesser;
    [SerializeField] private ConstantType _costant;
    [SerializeField] private bool isInPercantage = true;

    [SerializeField] private float _equalityTolerance = 0.5f; // Допуск для сравнения на равенство

    public override bool CheckTrigger(ICharacter character)
    {
        if (logging) Debug.Log($"Check trigger {this.name} started {_relationType} mode");

        ICharacter _charcterToCheck = GetTargetForTrigger(character);

        if (_charcterToCheck == null) Debug.LogError("NO CHARACTER");
        float actualValue;

        if (isInPercantage)
        {
            actualValue = _charcterToCheck.StatController.Stats[_statToCheck].GetValuePercentage();
        }
        else
        {
            actualValue = _charcterToCheck.StatController.Stats[_statToCheck].GetValuePercentage();
        }

        float compareWith = Constants.Get(_costant);
        
        bool result = CompareWithTolerance(actualValue, compareWith, _relationType, _equalityTolerance);

        if (logging) Debug.Log($"Check  {_statToCheck} value: {actualValue} is {_relationType} then {compareWith} result {result} ");



        //if (!character.GetTargetsVault().TryGetTargetEnemy(out GameObject enemy)) return false;


        return result;
    }

    private bool CompareWithTolerance(float actualValue, float compareWith, RelationType relation, float tolerance)
    {

        switch (relation)
        {
            case RelationType.Lesser:
                if (logging) Debug.Log($"Check trigger {this.name}: actualValue = {actualValue} compareWith= {compareWith}");
                return actualValue < compareWith;

            case RelationType.Equal:
                if (logging) Debug.Log($"Check trigger {this.name}: actualValue = {actualValue} compareWith= {compareWith}");
                return Mathf.Abs(actualValue - compareWith) <= tolerance;

            case RelationType.Greater:
                if (logging) Debug.Log($"Check trigger {this.name}: actualValue = {actualValue} compareWith= {compareWith}");
                return actualValue > compareWith;

            default:
                Debug.LogWarning($"Unknown RelationType: {relation}");
                return false;
        }
    }

}
