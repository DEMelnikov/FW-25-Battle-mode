using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilitiesPack", menuName = "FW25/Ability System/Abilities Pack")]
public class AbilitiesPack : ScriptableObject
{
    [SerializeField] private List<Ability> abilities = new();

    public List<Ability> Abilities { get => abilities;  }
}
