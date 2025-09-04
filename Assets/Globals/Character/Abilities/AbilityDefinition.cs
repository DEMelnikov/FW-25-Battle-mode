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

        [Tooltip("������������ �������� �����������, 0 � ����������/��� ������������")]
        public float duration = 0f;

        [Tooltip("������ ����� (��� ��������� ��������)")]
        public float tickPeriod = 0f;

        [Tooltip("�� ����� �����������")]
        public float cooldown = 0f;

        [Tooltip("����� �� �������� (�������� ����-����� � ��������� Attack)")]
        public bool looping = false;

        [Header("Economy")]
        public ResourceCost[] costs;

        [Header("Requirements")]
        public AbilityRequirementSO[] activateRequirements;

        [Header("Effects")]
        public AbilityEffectSO[] effects;
    }
}