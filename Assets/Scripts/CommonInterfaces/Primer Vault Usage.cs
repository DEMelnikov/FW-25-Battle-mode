using UnityEngine;
using static Test_TriggerUser;

public class Test_TriggerUser : MonoBehaviour
{
    public  StatesVault stateVault;// Ссылка на нужный vault-ассет

    [SONameDropdown(typeof(StatesVault))]
    public string itemName;

    private State runtimeState;      // Клон объекта, используемый во время игры

    public State _SelectedState;
    public string _description;

    [Header("Triggers Vault")]
    public TriggersVault triggersVault;// Ссылка на нужный vault-ассет

    [SONameDropdown(typeof(TriggersVault))]
    public string triggerName;
    private Trigger runtimeTrigger;

    public Trigger triggerSelected;
    public string _triggerDescription;

    void Start()
    {
        if (triggersVault == null)
        {
            Debug.LogError("TriggersVault не назначен");
            return;
        }

        if (stateVault == null)
        {
            Debug.LogError("ItemsVault не назначен");
            return;
        }

        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning("itemName пустое");
            return;
        }

        if (string.IsNullOrEmpty(triggerName))
        {
            Debug.LogWarning("TriggerName пустое");
            return;
        }

        // Получаем клон объекта из Vault по имени
        runtimeState = stateVault.GetCopyByName(itemName);
        runtimeTrigger = triggersVault.GetCopyByName(triggerName);

        //if (runtimeState == null)
        //{
        //    Debug.LogWarning($"Объект с именем {itemName} не найден в ItemsVault");
        //}
        //else
        //{
        //    Debug.Log($"Объект {runtimeState.name} успешно загружен из ItemsVault");
        //    // Дальше можно работать с runtimeItem
        //}

        _description = runtimeState.GetDescription();
        _triggerDescription = runtimeTrigger.GetDescription();
    }
}
