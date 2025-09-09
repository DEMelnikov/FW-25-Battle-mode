using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_CharacterStatsConfig", menuName = "FW25/Characters/Stats Config")]
public class SO_CharacterStatsConfig : ScriptableObject
{
    [System.Serializable]
    public class StatDefinition
    {
        public string Name;
        public StatTag Tag;
        public float BaseValue;
        public bool HasAlarm;
    }

    [SerializeField] private List<StatDefinition> _stats = new List<StatDefinition>();

    public List<StatDefinition> Stats => _stats;
}