using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "Characters/Character Settings")]
public class CharacterSettings : ScriptableObject
{
    [SerializeField] private SceneObjectTag _sceneObjectTag = SceneObjectTag.Hero;
    [SerializeField] private GameObject _defaultTarget = null;

    public SceneObjectTag SceneObjectTag => _sceneObjectTag;
    public GameObject DefaultTarget => _defaultTarget;
}