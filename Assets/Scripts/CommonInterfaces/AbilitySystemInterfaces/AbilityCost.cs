//using UnityEngine;

//namespace AbilitySystem.AbilityComponents
//{
//    [System.Serializable]
//    public struct AbilityCost
//    {
//        public enum CostType { Stat, Item }

//        public CostType type;
//        public string statName; // ��� ���� Stat
//        public float statCost;
//        public int itemId; // ��� ���� Item (��������)
//        public int itemCount;
//    }
//}

using UnityEngine;


public abstract class AbilityCost : ScriptableObject
{
    public abstract bool PayAbilityCost(ICharacter character);
    public abstract bool CanAffordCost(ICharacter character);
}
