using UnityEngine;

public class BaseState : ScriptableObject, INameable
{
    [SerializeField] [TextArea(2, 3)] private string _description;
    [SerializeField] private string stateId;
    public string StateId => stateId;
    public virtual void CheckTransitions(IStateMachine machine)
    {
        // ����� ����� ������ �������� ��������� ����� �����������
    }

    public string GetName()
    {
        return name; // �������� ������������� ScriptableObject
    }

    public string GetDescription() => _description;

}