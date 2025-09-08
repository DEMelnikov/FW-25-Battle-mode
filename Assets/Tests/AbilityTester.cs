// Простой скрипт для тестирования
using AbilitySystem;
using UnityEngine;

public class AbilityTester : MonoBehaviour
{
                     private AbilityController abilityController;
    [SerializeField] private GameObject testingObject;
    [SerializeField] private KeyCode testKey = KeyCode.A;

    private void Awake()
    {
        abilityController = testingObject.GetComponent<Character>().GetComponent<AbilityController>();
    }
    private void Update()
    {
        if (abilityController == null) {   return;}

        if (Input.GetKeyDown(testKey) && abilityController.GetAllAbilities().Count > 0)
        {
            var ability = abilityController.GetAllAbilities()[0];
            abilityController.TryActivateAbility(ability);
            Debug.Log($"Attempted to use: {ability.GetAbilityName()}");
        }

        //if (Input.GetKeyDown(testKey))
        //{
        //    //var ability = abilityController.GetAllAbilities()[0];
        //    //abilityController.TryActivateAbility(ability);
        //    Debug.Log($"Attempted to use: {abilityController.GetAllAbilities().Count}");
        //}
    }
}