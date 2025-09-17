using AbilitySystem.AbilityComponents;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/Vaults/ActionsVault")]
public class ActionsVault : ScriptableObject
{
    [SerializeField]
    private List<AbilityAction> items = new List<AbilityAction>();

    public IReadOnlyList<AbilityAction> Items => items;

    public AbilityAction GetByName(string name)
    {
        return items.FirstOrDefault(item => item.name == name);
    }

    public AbilityAction GetCopyByName(string name)
    {
        var original = GetByName(name);
        return original != null ? Instantiate(original) : null;
    }
}