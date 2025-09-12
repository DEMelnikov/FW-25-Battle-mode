[System.Serializable]
public class Transition: ITransition
{
    public Decision decision;
    public IState trueState;

    Decision ITransition.decision => decision;// throw new System.NotImplementedException();

    IState ITransition.trueState => trueState;//throw new System.NotImplementedException();

    //public IDecision ITransition.decision => decision;

    //IState ITransition.trueState => trueState;

    //public State falseState;
}