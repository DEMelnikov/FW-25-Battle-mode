using UnityEngine;
using UnityEngine.TextCore.Text;


public abstract class AbilityResolve : ScriptableObject
{
    [Header("Targeting:")]
    [SerializeField] protected AbilityTargets _targeting = AbilityTargets.SELF;
    [SerializeField]
    [Min(0)] protected float _targetRadius = 0f;
    [SerializeField] protected  bool logging = false;

    //public abstract void ApplyResolve(Character character, ActionOutcome outcome);
    public virtual void ApplyResolve(ICharacter character, int outcome) { }

    protected  ICharacter GetTargetForAbility(ICharacter character)
    {
        ICharacter _targetCharacter = null;

        switch (_targeting)
        {

            case AbilityTargets.SELF:
                _targetCharacter = character;
                break;

            case AbilityTargets.TARGETENEMY:
                if (logging) Debug.Log($"Resolve {this.name} Target to resolve is TargeteEnemy:");

                if (character.GetTargetsVault().TryGetTargetCharacter(out var _enemyCharacter))
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
    protected ICharacter GetTargetForAbility(ICharacter character, AbilityTargets manualSetTarget)
    {
        ICharacter _targetCharacter = null;

        switch (manualSetTarget)
        {

            case AbilityTargets.SELF:
                _targetCharacter = character;
                break;

            case AbilityTargets.TARGETENEMY:
                if (!character.GetTargetsVault().TryGetTargetCharacter(out var _enemyCharacter))
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
}
