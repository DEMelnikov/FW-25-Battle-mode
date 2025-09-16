using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/Vaults/StatesVault")]
public class StatesVault : ScriptableObject
{
    [SerializeField]
    private List<State> items = new List<State>();

    public IReadOnlyList<State> Items => items;

    public State GetByName(string name)
    {
        return items.FirstOrDefault(item => item.name == name);
    }

    public State GetCopyByName(string name)
    {
        var original = GetByName(name);
        return original != null ? Instantiate(original) : null;
    }
}
