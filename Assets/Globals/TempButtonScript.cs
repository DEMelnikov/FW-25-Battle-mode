using NUnit.Framework.Internal;
using UnityEngine;

public class TempButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject characterQQQ;
    [SerializeField] private GameObject characterEnemy;

    public void DealDamage()
    {
        if (SelectionManager.SelectedObject.GetComponent<CharacterStatsController>().
                Stats.TryGetValue(StatTag.Health, out var healthStat))
        {
            healthStat.AddToTmpModifier(-5f);
        }

        if (SelectionManager.SelectedObject.GetComponent<CharacterStatsController>().
         Stats.TryGetValue(StatTag.Energy, out var epStat))
        {
            epStat.AddToTmpModifier(-5f);
        }

        if (SelectionManager.OpponentObject.GetComponent<CharacterStatsController>().
        Stats.TryGetValue(StatTag.Health, out var EnhealthStat))
        {
            EnhealthStat.AddToTmpModifier(-10f);
        }

        if (SelectionManager.OpponentObject.GetComponent<CharacterStatsController>().
         Stats.TryGetValue(StatTag.Energy, out var EnepStat))
        {
            EnepStat.AddToTmpModifier(-25f);
        }
    }

    public void ReadAllStats()
    {
        var selection = SelectionManager.SelectedObject;
        if (selection == null) selection = SelectionManager.OpponentObject;
        if (selection == null) return;

        selection.GetComponent<CharacterStatsController>().LogingData();
    }

    public void TestAction()
    {
        var abilka = characterEnemy.GetComponent<Character>().GetAbilityController().GetAllAbilities();
        var ability = abilka[0];
        characterEnemy.GetComponent<Character>().GetAbilityController().TryActivateAbility(ability);
    }

    public void SetTarget()
    {
        characterQQQ.GetComponent<Character>().GetTargets().SetTargetEnemy(characterEnemy);
    }
}
