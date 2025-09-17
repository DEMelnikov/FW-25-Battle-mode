using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/AllTriggersPass")]
public class AllTriggersPassDecision : Decision
{

    public override bool Decide(IStateMachine machine)
    {
        if (logging) Debug.Log($"Start Decision {this.name}");
        var character = machine.Context.GetCharacter();
        //bool finalDecision = true;

        if(abilityTriggers.Count>0) return false;

        foreach (var trigger in abilityTriggers) 
        {
            if (!trigger.CheckTrigger(character))
            {
                if (logging) Debug.Log($"Decision {this.name} - check {trigger.name} - FAILED");
                return false;
            }
            else
            {
                if (logging) Debug.Log($"Decision {this.name} - check {trigger.name} - PASSED");
            }
        } 


        if (logging) Debug.Log($"Decision {this.name} passed fully");
        return true;
    }
}
