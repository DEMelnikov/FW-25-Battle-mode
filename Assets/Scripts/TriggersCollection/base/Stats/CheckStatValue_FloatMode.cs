using UnityEngine;

[CreateAssetMenu(fileName = "CheckStatValue_float", menuName = "FW25/Triggers/Stats/CheckStatValue (Float mode)")]
public class CheckStatValue_FloatMode : CheckStatValue
{
    [SerializeField] private  float _comapareWith;

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

        bool result = CompareWithTolerance(actualValue, _comapareWith, _relationType, _equalityTolerance);

        if (logging) Debug.Log($"Check  {_statToCheck} value: {actualValue} is {_relationType} then {_comapareWith} result {result} ");

        return result;
    }



}
