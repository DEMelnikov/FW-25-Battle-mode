using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/AllTriggersPass")]
public class AllTriggersPassDecision : Decision
{
    public override bool Decide(IStateMachine machine)
    {
        if (logging) Debug.Log($"Start Decision {this.name}");
        var character = machine.Context.GetCharacter();

        return base.Decide(machine);
    }
}
