// Простой скрипт для тестирования
using AbilitySystem;
using EditorAttributes;
using UnityEngine;
using UnityEngine.TextCore.Text;


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

    [Header("Action tests")]
    [SerializeField] private AbilityAction _actionToTest;

    [Header("Resolves tests")]
    [SerializeField] private AbilityResolve _resolveToTest;
    [SerializeField] private int outcome;


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
    public void GetAllCharacterStats()
    {
        AddEnemy();

        if (character == null)
        {
            Debug.LogError("No Character in Ability Tester");
        }

        //abilityToTest.TryActivateAbility(character, out int outcome);
        foreach(var item in character.StatController.Stats)
        {
            Debug.Log($"{character.ObjName} - {item.Key} - {item.Value.Value} / {item.Value.BaseValue}");
        }
        
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
        character.        TargetsVault.SetTargetEnemyCharacter(toSetEnemy.GetComponent<Character>());
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
        float oldvalue = character.StatController.Stats[_statToSetValue].Value;
        float oldPercentage = character.StatController.Stats[_statToSetValue].GetValuePercentage();

        character.
        StatController.Stats[_statToSetValue].TESTSSetValue(_newValue);

        float newValue = character.StatController.Stats[_statToSetValue].Value;
        float newPercentage = character.StatController.Stats[_statToSetValue].GetValuePercentage();
        Debug.Log($" old value ={oldvalue} {oldPercentage}% /// new value {newValue} {newPercentage}% ");
    }

    [Button]
    public void TestCost()
    {
        _costToTest.PayAbilityCost(character);
    }

    [Button]
    public void TestAction()
    {
        AddEnemy();
        _actionToTest.ExecuteAction(character);
    }

    [Button]
    public void TestResolve()
    {
        _resolveToTest.ApplyResolve(character, outcome);
    }
}