[System.Serializable]
public class Transition: ITransition
{
    public Decision decision;
    public State trueState;

    Decision ITransition.decision => throw new System.NotImplementedException();

    State ITransition.trueState => throw new System.NotImplementedException();
    //public State falseState;
}