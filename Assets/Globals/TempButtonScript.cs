using NUnit.Framework.Internal;
using UnityEngine;

public class TempButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject characterQQQ;

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
        SelectionManager.SelectedObject.GetComponent<CharacterStatsController>().LogingData();
    }
}
