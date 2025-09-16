using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/AllTriggersPass")]
public class AllTriggersPassDecision : Decision
{

    public override bool Decide(IStateMachine machine)
    {
        if (logging) Debug.Log($"Start Decision {this.name}");
        var character = machine.Context.GetCharacter();
        bool finalDecision = true;

        if(abilityTriggers.Count>0) return false;

        foreach (var trigger in abilityTriggers) 
        {
            if (!trigger.CheckTrigger(character)) finalDecision = false;
        }
        
        if (logging) Debug.Log($"Decision {this.name} passed");
        return finalDecision;
    }
}
