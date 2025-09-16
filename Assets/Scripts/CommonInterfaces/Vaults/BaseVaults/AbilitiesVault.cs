using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "FW25/Vaults/AbilitiesVault")]
public class AbilitiesVault : ScriptableObject
{
    [SerializeField]
    private List<Ability> items = new List<Ability>();

    public IReadOnlyList<Ability> Items => items;

    public Ability GetByName(string name)
    {
        return items.FirstOrDefault(item => item.name == name);
    }

    public Ability GetCopyByName(string name)
    {
        var original = GetByName(name);
        return original != null ? Instantiate(original) : null;
    }
}
