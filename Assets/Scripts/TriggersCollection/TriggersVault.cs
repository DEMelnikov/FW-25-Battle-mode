using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;

[CreateAssetMenu(fileName = "TriggersVault", menuName = "FW25/Vaults/TriggersVault")]
public class TriggersVault : ScriptableObject
{
    [SerializeField] public List<Trigger> triggers;
    public static TriggersVault Instance { get; private set; }

    private void OnEnable()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple TriggersVault instances found. Only one Instance is supported.");
        }
        else
        {
            Instance = this;
        }
    }

    public Trigger GetTriggerByName(string name)
    {
        return triggers.FirstOrDefault(a => a.GetTriggerName() == name);
    }

    public Trigger GetTriggerCopyByName(string name)
    {
        var original = GetTriggerByName(name);
        if (original != null)
        {
            return original.Clone();
        }
        return null;
    }
}
