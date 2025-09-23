using UnityEngine;

public class BaseState : ScriptableObject, INameable
{
    [SerializeField] [TextArea(2, 3)] private string _description;
    [SerializeField] private string stateId;
    public string StateId => stateId;
    public virtual void CheckTransitions(IStateMachine machine)
    {
        // Здесь будет логика проверки переходов между состояниями
    }

    public string GetName()
    {
        return name; // свойство родительского ScriptableObject
    }

    public string GetDescription() => _description;

}