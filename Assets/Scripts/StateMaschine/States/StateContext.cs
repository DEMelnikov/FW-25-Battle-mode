using UnityEngine;

public class StateContext : IStateContext
{
    public GameObject Owner { get; private set; }
    private Character _character;
    private IAbilityController _abilityController;
    
    //public AbilityController _abilityController;
    //public Transform PlayerTarget { get; set; }
    //public Animator Animator { get; private set; }
    //public Rigidbody Physics { get; private set; }

    public StateContext(GameObject owner)
    {
        Owner = owner;
        owner.TryGetComponent<Character>(out _character);
        _abilityController = owner.GetComponent<IAbilityController>();
        //Animator = owner.GetComponent<Animator>();
        //Physics = owner.GetComponent<Rigidbody>();
    }

    public ICharacter GetCharacter() => _character;
    public IAbilityController GetAbilityController () => _abilityController;
    // Вспомогательные методы
    //public bool HasPlayerTarget => PlayerTarget != null;

    //public float GetDistanceToPlayer()
    //{
    //    if (!HasPlayerTarget) return Mathf.Infinity;
    //    return Vector3.Distance(Owner.transform.position, PlayerTarget.position);
    //}

    //public Vector3 GetDirectionToPlayer()
    //{
    //    if (!HasPlayerTarget) return Vector3.zero;
    //    return (PlayerTarget.position - Owner.transform.position).normalized;
    //}
}