// CommonInterfaces/ISelectableCharacter.cs
using UnityEngine;

public interface ISelectableCharacter
{
    SceneObjectTag SceneObjectTag { get; }
    GameObject GetSelectedTarget();
    //GameObject gameObject { get; }
}