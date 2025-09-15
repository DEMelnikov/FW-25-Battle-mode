using UnityEngine;
using static Test_TriggerUser;

public class Test_TriggerUser : MonoBehaviour
{
    public  StatesVault stateVault;// ������ �� ������ vault-�����

    [SONameDropdown(typeof(StatesVault))]
    public string itemName;

    private State runtimeState;      // ���� �������, ������������ �� ����� ����

    public State _SelectedState;
    public string _description;

    [Header("Triggers Vault")]
    public TriggersVault triggersVault;// ������ �� ������ vault-�����

    [SONameDropdown(typeof(TriggersVault))]
    public string triggerName;
    private Trigger runtimeTrigger;

    public Trigger triggerSelected;
    public string _triggerDescription;

    void Start()
    {
        if (triggersVault == null)
        {
            Debug.LogError("TriggersVault �� ��������");
            return;
        }

        if (stateVault == null)
        {
            Debug.LogError("ItemsVault �� ��������");
            return;
        }

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning("itemName ������");
            return;
        }

        if (string.IsNullOrEmpty(triggerName))
        {
            Debug.LogWarning("TriggerName ������");
            return;
        }

        // �������� ���� ������� �� Vault �� �����
        runtimeState = stateVault.GetCopyByName(itemName);
        runtimeTrigger = triggersVault.GetCopyByName(triggerName);

        //if (runtimeState == null)
        //{
        //    Debug.LogWarning($"������ � ������ {itemName} �� ������ � ItemsVault");
        //}
        //else
        //{
        //    Debug.Log($"������ {runtimeState.name} ������� �������� �� ItemsVault");
        //    // ������ ����� �������� � runtimeItem
        //}

        _description = runtimeState.GetDescription();
        _triggerDescription = runtimeTrigger.GetDescription();
    }
}
