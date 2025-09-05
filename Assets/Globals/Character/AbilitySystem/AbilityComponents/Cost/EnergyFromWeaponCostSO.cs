using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "EnergyFromWeapon", menuName = "FW25/Ability System/Ability Cost/Energy From Weapon")]
    public class EnergyFromWeaponCost : AbilityCost
    {
        [SerializeField] [Min (0.1f)] private float _energyCost = 1f;
        //TODO - взять стоимость от оружия и набрать бафов

        public override bool PayAbilityCost(Character character)
        {
            CharacterStatsController heroStats = character.GetStatsController();

            if (CanAffordCost(character))
            {
                heroStats.Stats[StatTag.Energy].AddToTmpModifier(_energyCost*-1);
                return true;
            }
            else return false;
        }

        public override bool CanAffordCost(Character character)
        {
            CharacterStatsController heroStats = character.GetStatsController();
            if (heroStats.Stats[StatTag.Energy].Value > _energyCost)  return true; else return false;
        }
    }
}
