using UnityEngine;

[CreateAssetMenu(menuName = "FW25/State Machine/Decisions/Time Elapsed Decision")]
public class TimeElapsedDecision : Decision
{
    [SerializeField] private float requiredTime = 2f;

    private float _timer;

    public override void OnEnter(StateMachine machine)
    {
        _timer = 0f;
        Debug.Log("TimeElapsedDecision: Timer reset");
    }

    public override bool Decide(StateMachine machine)
    {
        _timer += Time.deltaTime;
        Debug.Log($"TimeElapsedDecision: Timer = {_timer:F1}/{requiredTime}");
        return _timer >= requiredTime;
    }

    public override void OnExit(StateMachine machine)
    {
        Debug.Log("TimeElapsedDecision: Exit");
    }

    public override void OnUpdate(StateMachine machine)
    {
        // ƒополнительна€ логика обновлени€, если нужна
    }
}