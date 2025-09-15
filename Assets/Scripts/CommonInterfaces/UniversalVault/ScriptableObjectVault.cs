using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class ScriptableObjectVault<T> : ScriptableObject where T : ScriptableObject
{
    public List<T> items;

    public T GetByName(string name)
    {
        return items.FirstOrDefault(i => i.name == name);
    }

    public T GetCopyByName(string name)
    {
        var original = GetByName(name);
        return original != null ? Instantiate(original) : null;
    }
}
