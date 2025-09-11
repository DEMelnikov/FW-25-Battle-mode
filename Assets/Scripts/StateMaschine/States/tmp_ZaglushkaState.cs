using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/States/Zaglushka State")]
public class tmp_ZaglushkaState : StateWithTransitions
{
    [SerializeField] private string _message;
    public override void OnEnter(IStateMachine machine)
    {
        Debug.LogWarning($"{machine.Context.Owner.name} Enters to Zaglushka State _ name: {_message}");
        base.OnEnter(machine);
    }

    public override void OnExit(IStateMachine machine)
    {
        base.OnExit(machine);
    }

    public override void OnFixedUpdate(IStateMachine machine)
    {
        base.OnFixedUpdate(machine);
    }

    public override void OnUpdate(IStateMachine machine)
    {
        base.OnUpdate(machine);
    }

    protected override void CheckTransitions(IStateMachine machine)
    {
        base.CheckTransitions(machine);
    }
}
