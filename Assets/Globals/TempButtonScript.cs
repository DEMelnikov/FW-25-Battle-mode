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
    }
}
