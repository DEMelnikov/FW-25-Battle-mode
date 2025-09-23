//using UnityEngine;

//namespace AbilitySystem.AbilityComponents
//{
//    [System.Serializable]
//    public struct AbilityCost
//    {
//        public enum CostType { Stat, Item }

//        public CostType type;
//        public string statName; // Для типа Stat
//        public float statCost;
//        public int itemId; // Для типа Item (заглушка)
//        public int itemCount;
//    }
//}

using UnityEngine;


public abstract class AbilityCost : ScriptableObject
{
    public abstract bool PayAbilityCost(ICharacter character);
    public abstract bool CanAffordCost(ICharacter character);
}
