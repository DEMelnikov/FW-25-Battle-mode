using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    //public enum ActionResult { Success, Failure, CriticalSuccess, CriticalFailure }

    [System.Serializable]
    public struct ActionOutcome
    {
        //public ActionResult result;
        public int successCont; // 0-1 или другое значение для оценки степени успеха
        public string message;
    }

    public abstract class AbilityAction : ScriptableObject
    {
        //public abstract ActionOutcome ExecuteAction(Character character);
        public abstract int ExecuteAction(Character character);
    }
}