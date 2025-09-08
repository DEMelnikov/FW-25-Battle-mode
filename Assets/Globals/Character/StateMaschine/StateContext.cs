using UnityEngine;

public class StateContext
{
    public GameObject Owner { get; private set; }
    //public Transform PlayerTarget { get; set; }
    //public Animator Animator { get; private set; }
    //public Rigidbody Physics { get; private set; }

    public StateContext(GameObject owner)
    {
        Owner = owner;
        //Animator = owner.GetComponent<Animator>();
        //Physics = owner.GetComponent<Rigidbody>();
    }

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