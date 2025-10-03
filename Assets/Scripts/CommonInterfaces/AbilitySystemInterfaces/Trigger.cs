using UnityEngine;

//TODO - move class to Triggers Vault
public abstract class Trigger : ScriptableObject
{

    [SerializeField] [TextArea(3, 5)] protected string _description;
    [SerializeField] protected string _name;

    [Header("Targeting:")]
    [SerializeField] protected AbilityTargets _targeting = AbilityTargets.SELF;
                     protected ICharacter _targetCharacter;

    [Header("Настройки:")]
    [SerializeField] protected bool logging = true;

    public abstract bool CheckTrigger(ICharacter character);
    public string GetTriggerName() { return _name; } //TODO - seal this

    public virtual Trigger Clone() //TODO - seal this
        {
            // Реализация клона через Instantiate
            return Instantiate(this);
        }

    public string GetDescription() => _description;


    protected ICharacter GetTargetForTrigger(ICharacter character)
    {
        ICharacter _targetCharacter = null;

        switch (_targeting)
        {

            case AbilityTargets.SELF:
                _targetCharacter = character;
                break;

            case AbilityTargets.TARGETENEMY:
                if (logging) Debug.Log($"Resolve {this.name} Target to resolve is TargeteEnemy:");

                if (character.TargetsVault.TryGetTargetCharacter(out var _enemyCharacter))
                {
                    _targetCharacter = _enemyCharacter;
                }
                else
                {
                    Debug.LogError($"{character.GetGameObject.name} - has no TargetEnemy! ");
                }
                break;

            default:
                Debug.LogError($"{character.GetGameObject.name} - Unknown target for trigger {_targeting}");
                break;
        }

        return _targetCharacter;
    }
    protected ICharacter GetTargetForTrigger(ICharacter character, AbilityTargets manualSetTarget)
    {
        ICharacter _targetCharacter = null;

        switch (manualSetTarget)
        {

            case AbilityTargets.SELF:
                _targetCharacter = character;
                break;

            case AbilityTargets.TARGETENEMY:
                if (!character.TargetsVault.TryGetTargetCharacter(out var _enemyCharacter))
                {
                    Debug.LogError($"{character.GetGameObject.name} - has no TargetEnemy! ");
                    _targetCharacter = _enemyCharacter;
                }
                break;

            default:
                Debug.LogError($"{character.GetGameObject.name} - Unknown target for trigger {manualSetTarget}");
                break;
        }

        return _targetCharacter;
    }


    //public bool GetLoggingState() {  return logging; }
}
