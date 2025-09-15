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

    void Start()
    {
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

        // �������� ���� ������� �� Vault �� �����
        runtimeState = stateVault.GetCopyByName(itemName);

        if (runtimeState == null)
        {
            Debug.LogWarning($"������ � ������ {itemName} �� ������ � ItemsVault");
        }
        else
        {
            Debug.Log($"������ {runtimeState.name} ������� �������� �� ItemsVault");
            // ������ ����� �������� � runtimeItem
        }

        _description = runtimeState.GetDescription();
    }
}
