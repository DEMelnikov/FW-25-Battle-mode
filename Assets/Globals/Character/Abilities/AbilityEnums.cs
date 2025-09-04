using System;
using UnityEngine;

namespace FW25.Abilities
{
    [Flags]
    public enum AbilityTag
    {
        None = 0,
        Offensive = 1 << 0,
        Defensive = 1 << 1,
        Utility = 1 << 2,
        Healing = 1 << 3,
        Weapon_Sword = 1 << 4,
        Weapon_Bow = 1 << 5,
        Attack = 1 << 6,
        Passive = 1 << 7,
    }

    public enum AbilityKind
    {
        Passive,
        Active,
        Triggered
    }

    public enum AbilityTriggerType
    {
        None,
        OnLearn,
        OnEquip,
        OnUnequip,
        OnStateEnter_Attack,
        OnStateExit_Attack,
        OnHit,
        OnDamaged,
        OnKill,
        OnDeath,
        Manual
    }

    public enum ResourceType
    {
        Health,
        Stamina,
        Energy,
        Mana
    }

    [Serializable]
    public struct ResourceCost
    {
        public ResourceType type;
        public float amount;
        public bool perTick; // true Ч списывать на каждый тик, false Ч только при старте
    }
}