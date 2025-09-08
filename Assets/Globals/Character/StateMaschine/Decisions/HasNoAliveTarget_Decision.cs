using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/Has No Alive Target Decision")]
public class HasNoAliveTarget_DecisionSO : Decision
{
    public override bool Decide(StateMachine machine)
    {
        if (logging) Debug.Log("Start Decision Has No Alive Target");

        var character = machine.Context.GetCharacter();
        var target = character.GetSelectedTarget();
        if (target == null)
        {
            if (logging) Debug.Log($"Decision: {machine.Context.Owner.name} has no target");
            return true;
        }

        return false;
    }
}
