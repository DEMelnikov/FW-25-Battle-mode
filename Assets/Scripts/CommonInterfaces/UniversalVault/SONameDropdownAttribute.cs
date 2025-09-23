using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class SONameDropdownAttribute : PropertyAttribute
{
    public Type VaultType { get; }

    public SONameDropdownAttribute(Type vaultType)
    {
        VaultType = vaultType;
    }
}
