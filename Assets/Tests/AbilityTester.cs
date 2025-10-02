// Простой скрипт для тестирования
using AbilitySystem;
using UnityEngine;
using EditorAttributes;


public class AbilityTester : MonoBehaviour
{
                     //private AbilityController abilityController;
    [SerializeField] private GameObject testingObject;
    [SerializeField] private GameObject toSetEnemy;

    [SerializeField] private Ability    abilityToTest;
    [SerializeField] private Trigger    triggerToTest;    
    
                     private Character  character;

    [Header("Set Stat")]
    [SerializeField] private StatTag _statToSetValue = StatTag.ENERGY;
    [SerializeField] private float   _newValue = 10f;

    [Header("Cost tests")]
    [SerializeField] private AbilityCost _costToTest; 
    

    //[SerializeField] private Character  enemyCharacter;


    //[SerializeField] private KeyCode testKey = KeyCode.A;

    private void Awake()
    {
        //abilityController = testingObject.GetComponent<Character>().GetComponent<AbilityController>();
        character = testingObject.GetComponent<Character>();
    }
    private void Update()
    {
    }
    
    [Button]
    public void TestAbility()
    {
        AddEnemy();

        if (character == null)
        {
            Debug.LogError("No Character in Ability Tester");
        }
        abilityToTest.TryActivateAbility(character,out int outcome);
    }

    [Button]
    public void AddEnemy()
    {
        character.GetTargetsVault().SetTargetEnemyCharacter(toSetEnemy.GetComponent<Character>());
        //character.GetTargetsVault().TryGetTargetCharacter(out ICharacter enemyICharacter);
        //enemyCharacter = enemyICharacter;
    }

    [Button]
    public void TestAttack()
    {
        character.AbilityController.TryExecuteAttackAbility();
    }

    [Button]
    public void TestTrigger()
    {
        triggerToTest.CheckTrigger(character);
    }

    [Button]
    public void SetNewValue()
    {
        float oldvalue = character.GetStatsController().Stats[_statToSetValue].Value;
        float oldPercentage = character.GetStatsController().Stats[_statToSetValue].GetValuePercentage();

        character.GetStatsController().Stats[_statToSetValue].TESTSSetValue(_newValue);

        float newValue = character.GetStatsController().Stats[_statToSetValue].Value;
        float newPercentage = character.GetStatsController().Stats[_statToSetValue].GetValuePercentage();
        Debug.Log($" old value ={oldvalue} {oldPercentage}% /// new value {newValue} {newPercentage}% ");
    }

    [Button]
    public void TestCost()
    {
        _costToTest.PayAbilityCost(character);
    }
}