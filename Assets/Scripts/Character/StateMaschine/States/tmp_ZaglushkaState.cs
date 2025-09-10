using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Zaglushka State")]
public class tmp_ZaglushkaState : StateWithTransitions
{
    [SerializeField] private string _message;
    public override void OnEnter(StateMachine machine)
    {
        Debug.LogWarning($"{machine.Context.Owner.name} Enters to Zaglushka State _ name: {_message}");
        base.OnEnter(machine);
    }

    public override void OnExit(StateMachine machine)
    {
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(StateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(StateMachine machine)
    {
        base.OnUpdate(machine);
    }

    protected override void CheckTransitions(StateMachine machine)
    {
        base.CheckTransitions(machine);
    }
}
