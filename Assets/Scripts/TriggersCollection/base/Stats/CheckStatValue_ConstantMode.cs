
using UnityEngine;

[CreateAssetMenu(fileName = "CheckStatValue_Constant", menuName = "FW25/Triggers/Stats/CheckStatValue (Constant mode)")]

//Constant copair mode

public class CheckStatValue_ConstantMode : CheckStatValue
{
    [SerializeField] private ConstantType _costant;

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



}
