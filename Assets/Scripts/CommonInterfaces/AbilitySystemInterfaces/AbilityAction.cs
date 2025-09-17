using UnityEngine;


    //public enum ActionResult { Success, Failure, CriticalSuccess, CriticalFailure }

[System.Serializable]

public abstract class AbilityAction : ScriptableObject, IAction
{
    //public abstract ActionOutcome ExecuteAction(Character character);
    [SerializeField] [TextArea(2,5)] protected string _description;
    [SerializeField] protected bool logging = true;
    public virtual int ExecuteAction(ICharacter character) { return 0; }
}


public struct ActionOutcome
{
    //public ActionResult result;
    public int successCont; // 0-1 или другое значение для оценки степени успеха
    public string message;

}
