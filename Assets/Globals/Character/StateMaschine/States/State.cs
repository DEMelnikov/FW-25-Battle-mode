using UnityEngine;

public abstract class State : ScriptableObject
{
    [SerializeField] [TextArea(2,3)] protected string _description;
    [SerializeField] public bool logging = true;
    public virtual void OnEnter(StateMachine machine) { }
    public virtual void OnUpdate(StateMachine machine) { }
    public virtual void OnFixedUpdate(StateMachine machine) { }
    public virtual void OnExit(StateMachine machine) { }

    // ������ ��� �������� ���������
    protected virtual void CheckTransitions(StateMachine machine)
    {
        // ����� ����� ������ �������� ��������� ����� �����������
    }
}