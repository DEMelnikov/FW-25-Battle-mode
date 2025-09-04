using UnityEngine;

namespace FW25.Abilities
{
    [CreateAssetMenu(menuName = "FW25/Abilities/Ability")]
    public class AbilityDefinition : ScriptableObject
    {
        [Header("Meta")]
        public string id;
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;
        public AbilityTag tags;

        [Header("Behavior")]
        public AbilityKind kind = AbilityKind.Active;
        public AbilityTriggerType trigger = AbilityTriggerType.Manual;
        public bool unlockedByDefault = false;

        [Tooltip("Длительность активной способности, 0 — мгновенная/без длительности")]
        public float duration = 0f;

        [Tooltip("Период тиков (для лупящихся эффектов)")]
        public float tickPeriod = 0f;

        [Tooltip("КД между активациями")]
        public float cooldown = 0f;

        [Tooltip("Можно ли лупиться (например авто-атака в состоянии Attack)")]
        public bool looping = false;

        [Header("Economy")]
        public ResourceCost[] costs;

        [Header("Requirements")]
        public AbilityRequirementSO[] activateRequirements;

        [Header("Effects")]
        public AbilityEffectSO[] effects;
    }
}