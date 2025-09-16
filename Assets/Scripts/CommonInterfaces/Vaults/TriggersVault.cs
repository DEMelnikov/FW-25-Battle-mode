using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/Vaults/TriggersVault")]
public class TriggersVault : ScriptableObject
{
    [SerializeField]
    private List<Trigger> items = new List<Trigger>();

    public IReadOnlyList<Trigger> Items => items;

    public Trigger GetByName(string name)
    {
        return items.FirstOrDefault(item => item.name == name);
    }

    public Trigger GetCopyByName(string name)
    {
        var original = GetByName(name);
        return original != null ? Instantiate(original) : null;
    }
}