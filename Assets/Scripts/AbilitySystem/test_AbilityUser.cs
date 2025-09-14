using UnityEngine;

public class test_AbilityUser : MonoBehaviour
{
    [AbilityName]
    public string abilityName;
    private IAbility runtimeAbility;
    [SerializeField] private Ability qqq;

    void Start()
    {
        runtimeAbility = AbilitiesVault.Instance.GetAbilityCopyByName(abilityName);
        qqq = runtimeAbility as Ability;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
