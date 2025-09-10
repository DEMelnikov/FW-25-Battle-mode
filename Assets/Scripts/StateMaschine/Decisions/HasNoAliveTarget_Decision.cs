using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/Has No Alive Target Decision")]
public class HasNoAliveTarget_DecisionSO : Decision
{
    public override bool Decide(IStateMachine machine)
    {
        if (logging) Debug.Log("Start Decision Has No Alive Target");

        var character = machine.Context.GetCharacter();

        if (character.GetTargets().HasTargetEnemy())
        {
            if (logging) Debug.Log($"Decision: {machine.Context.Owner.name} has no target");
            return true;
        }

        return false;
    }
}
