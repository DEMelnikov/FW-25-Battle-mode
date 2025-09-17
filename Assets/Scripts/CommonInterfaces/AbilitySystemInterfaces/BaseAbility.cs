using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class BaseAbility : ScriptableObject
{
    public abstract bool HasTag(string tag);
    public abstract bool CanAfford(ICharacter character);
    protected virtual bool PayAllCost(ICharacter character) { return false; }
    public virtual bool GetLoggingState() { return false; }
    public string GetAbilityName() { return ""; }
    public virtual IAction GetAbilityAction() { return null; }
    public virtual bool TryActivateAbility (ICharacter character, out int outcome)
    {
        outcome = 0;
        return false;
    }

    protected virtual bool CheckTriggersReady(ICharacter character) {  return false; }

}