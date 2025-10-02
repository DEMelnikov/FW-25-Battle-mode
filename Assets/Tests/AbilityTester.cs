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
    
    
    [SerializeField] private Character  character;
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
}