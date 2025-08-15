using UnityEngine;

public class State
{
    protected Character _character;
    protected StateMaschine _stateMaschine;

    public State(Character character, StateMaschine stateMaschine)
    {
        this._character = character;
        this._stateMaschine = stateMaschine;
    }

    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicUpdate() { }
    //public virtual void AnimationTriggerEvent(_character.AnimationTriggerType triggerType) { }




}
