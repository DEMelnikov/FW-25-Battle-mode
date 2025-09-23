using Codice.Client.BaseCommands;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/AllTriggersPass")]
public class AllTriggersPassDecision : Decision
{

    public override bool Decide(IStateMachine machine)
    {
        //Debug.Break();
        Initialize();

        if (logging) Debug.Log($"Start Decision {this.name}");
        var character = machine.Context.GetCharacter();
        //bool finalDecision = true;

        if (abilityTriggers.Count == 0)
        {
            if (logging) Debug.Log($"Decision {this.name} - no triigers found -  result FAILED");
            return false;
        }

        if (logging) Debug.Log($"Decision {this.name} - has {abilityTriggers.Count} triggers start to check:");
        //Debug.Break();
        foreach (var trigger in abilityTriggers) 
        {
            //if (trigger == null) continue; // защита от MissingReferenceException

            if (logging) Debug.Log($"Decision {this.name} - *!*!*!* {trigger.name} in process:");

            if (!trigger.CheckTrigger(character))
            {
                if (logging) Debug.Log($"Decision {this.name} - check {trigger.name} - FAILED");
                if (logging) Debug.Log($"Decision {this.name} FAILED fully - *!*!*!*");
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
