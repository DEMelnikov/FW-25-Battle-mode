using NUnit.Framework.Internal;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

public class TempButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject characterQQQ;
    [SerializeField] private GameObject characterEnemy;
    [SerializeField] private PlayerState playerState = PlayerState.IDLE;


    [SerializeField] protected List<Trigger> abilityTriggers = new List<Trigger>(); //TODO remove SerializeField


    private void Awake()
    {

    }

    public void TestTriggers()
    {
        Debug.LogWarning($"**************Button Pressed**************************");
        //var machine = characterQQQ.GetComponent<ICharacter>().GetStateMachine();
        Debug.Log($"Start TEST triggers {this.name}");

        var character = characterQQQ.GetComponent<ICharacter>();
        //bool finalDecision = true;

        if (abilityTriggers.Count == 0) return;

        foreach (var trigger in abilityTriggers)
        {
            if (!trigger.CheckTrigger(character))
            {
                Debug.Log($"TEST BUTTON - check {trigger.name} - FAILED");
                return;
            }
            Debug.Log($"TEST BUTTON - check {trigger.name} - PASSED");
        }
    }

    public void DealDamage()
    {
        //if (SelectionManager.SelectedObject.GetComponent<CharacterStatsController>().
        //        Stats.TryGetValue(StatTag.Health, out var healthStat))
        //{
        //    healthStat.AddToTmpModifier(-5f);
        //}

        //if (SelectionManager.SelectedObject.GetComponent<CharacterStatsController>().
        // Stats.TryGetValue(StatTag.Energy, out var epStat))
        //{
        //    epStat.AddToTmpModifier(-5f);
        //}

        //if (SelectionManager.OpponentObject.GetComponent<CharacterStatsController>().
        //Stats.TryGetValue(StatTag.Health, out var EnhealthStat))
        //{
        //    EnhealthStat.AddToTmpModifier(-10f);
        //}

        //if (SelectionManager.OpponentObject.GetComponent<CharacterStatsController>().
        // Stats.TryGetValue(StatTag.Energy, out var EnepStat))
        //{
        //    EnepStat.AddToTmpModifier(-25f);
        //}
    }

    public void ReadAllStats()
    {
        //    var selection = SelectionManager.SelectedObject;
        //    if (selection == null) selection = SelectionManager.OpponentObject;
        //    if (selection == null) return;

        //    selection.GetComponent<CharacterStatsController>().LogingData();
        //}

        //public void TestAction()
        //{
        //    var abilka = characterEnemy.GetComponent<Character>().GetAbilityController().GetAllAbilities();
        //    var ability = abilka[0];
        //    characterEnemy.GetComponent<Character>().GetAbilityController().TryActivateAbility(ability);
        //}

        //public void SetTarget()
        //{
        //    characterQQQ.GetComponent<Character>().GetTargets().SetTargetEnemy(characterEnemy);
    }

    public void SetSPUMAnimation()
    {
        characterQQQ.GetComponent<ICharacter>().TESTTEST();//SPUM_PF.PlayAnimation(playerState, 0);
    }


}
