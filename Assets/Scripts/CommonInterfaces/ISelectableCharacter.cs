// CommonInterfaces/ISelectableCharacter.cs
using UnityEngine;

public interface ISelectableCharacter
{
    SceneObjectTag SceneObjectTag { get; }
    //GameObject GetSelectedTarget();
    ICharacterTargetsVault TargetsVault { get; }
    //GameObject gameObject { get; }
}