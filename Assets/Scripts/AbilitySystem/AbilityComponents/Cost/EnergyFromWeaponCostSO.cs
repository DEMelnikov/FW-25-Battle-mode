using UnityEngine;

namespace AbilitySystem.AbilityComponents
{
    [CreateAssetMenu(fileName = "EnergyFromWeapon", menuName = "FW25/Ability System/Ability Cost/Energy From Weapon")]


    public class EnergyFromWeaponCost : AbilityCost
    {
        [SerializeField] [Min (0.1f)] private float _energyCost = 1f;
        [SerializeField] private bool logging = true;

        //TODO - ����� ��������� �� ������ � ������� �����

        public override bool PayAbilityCost(Character character)
        {
            if (logging) Debug.Log($"Check payCost EnergyFromWeaponCost: check started");

            CharacterStatsController heroStats = character.GetStatsController();

            if (heroStats == null)
            {
                if (logging) Debug.Log($"Check payCost EnergyFromWeaponCost: can't get Hero stats");
                return false;
            }
            
            if (logging) Debug.Log($"Check payCost EnergyFromWeaponCost: Got Hero stats");
            
;
            if (logging) Debug.Log($"Check payCost CanAffordCost: check started");

            if (CanAffordCost(character))
            {
                heroStats.Stats[StatTag.Energy].AddToTmpModifier(_energyCost * -1);
                if (logging) Debug.Log($"Check payCost EnergyFromWeaponCost: check finished success");
                return true;
            }

            if (logging) Debug.Log($"Check payCost EnergyFromWeaponCost: check finished unsuccess");
            return false;
        }

        public override bool CanAffordCost(Character character)
        {
            CharacterStatsController heroStats = character.GetStatsController();
            if (heroStats.Stats[StatTag.Energy].Value > _energyCost)  return true; else return false;
        }
    }
}
