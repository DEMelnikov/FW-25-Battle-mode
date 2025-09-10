using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/Has Tagged Target Decision")]
public class HasTargetTaged_DecisionSO : Decision
{
    [SerializeField] private SceneObjectTag _targetTag = SceneObjectTag.Enemy;

    public override bool Decide(IStateMachine machine)
    {
        if (logging) Debug.Log("Start Decision HasTargetTaged");

        var character = machine.Context.GetCharacter();
        var target = character.GetSelectedTarget();
        if (target == null) 
        {
            if (logging) Debug.Log($"Decision: {machine.Context.Owner.name} has no target");
            return false;
        }

        // Защита 4: Безопасное получение компонента
        Character targetCharacter = target.GetComponent<Character>();
        if (targetCharacter == null)
        {
            if (logging) Debug.Log($"Decision: no Character component at target object");
            return false;
        }

        if (logging) Debug.Log($"Decision: get Target sceneObjectTag: {targetCharacter.SceneObjectTag} ");

        // Защита 6: Проверка тега с null-check
        bool tagMatches = targetCharacter.SceneObjectTag == _targetTag;
        if (logging) Debug.Log($"Decision: get tagMatches: {tagMatches} ");

        if (logging)
        {
            Debug.Log($"Decision: " +
                     $"target tag = {targetCharacter.SceneObjectTag}, " +
                     $"required tag = {_targetTag}, " +
                     $"result = {(tagMatches ? "PASSED+++" : "FAILED---")}");
        }

        return tagMatches;
    }
}
