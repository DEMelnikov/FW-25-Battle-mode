using NUnit.Framework;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Ability_subscribe", menuName = "FW25/Ability System/Ability/Ability With Stat Subscribe")]
public class AbilityWithSubscribe : Ability
{
    [Header("Subscribe types:")]
    [SerializeField] private bool subscribeToStats = false;
                     private bool subscribeFSM     = false;

    //TODO: возможно здесь будет List - сделать альтер

    [SerializeField] private List<StatTag> _statsToSubscribe = new();
                     private List<FSMStates> _statesToSubscribe = new();
    public override void IniTialize(ICharacter character)
    {
        base.IniTialize(character);

        bool needSubscribeTag =
            (subscribeToStats && _statsToSubscribe.Count > 0) ||
            (subscribeFSM && _statesToSubscribe.Count > 0);

        if (needSubscribeTag)
        {
            if (!this.HasTag(AbilityTag.WITHSUBSCRIBE))
                this.AbilityTags.Add(AbilityTag.WITHSUBSCRIBE);
        }
        else
        {
            if (this.HasTag(AbilityTag.WITHSUBSCRIBE))
                this.AbilityTags.Remove(AbilityTag.WITHSUBSCRIBE);
        }

    }
    public override void Subscribe(ICharacter character)
    {
        base.Subscribe(character);
        if (character == null || IsSubscribed || !HasTag(AbilityTag.WITHSUBSCRIBE)) return;

        if (subscribeToStats)
        {
            foreach (var item in _statsToSubscribe)
            {
                if (character.StatController.Stats.TryGetValue(item, out var foundStat))
                {
                    foundStat.OnValueChanged += OnStatValueChanged;
                }
            }
        }

        //TODO: SubscribeToFSM - Hero?

        //if (subscribeFSM)
        //{
        //    foreach (var item in _statsToSubscribe)
        //    {
        //        if (character.StatController.Stats.TryGetValue(item, out var foundStat))
        //        {
        //            foundStat.OnValueChanged += OnStatValueChanged;
        //        }
        //    }
        //}



        //Debug.LogError($"{_character.GetGameObject.name} - Ability Subscribed to STAT!!!");
        IsSubscribed = true;
    }

    public override void Unsubscribe(ICharacter character)
    {
        base.Unsubscribe(character);
        if (!IsSubscribed) return;

        foreach (var item in _statsToSubscribe)
        {
            if (character.StatController.Stats.TryGetValue(item, out var foundStat))
            {
                foundStat.OnValueChanged -= OnStatValueChanged;
            }
        }

        
        IsSubscribed = false;
    }

    private void OnStatValueChanged(IStat stat, float oldValue, float newValue)
    {
        if (logging)
        {
            if(logging) Debug.Log($"{abilityName}: Stat {stat.Name} changed from {oldValue} to {newValue}");
        }

        //if (newValue<=0) 
            EventHandler();
    }

    private void OnFSMStateChanged(FSMStates oldValue, FSMStates newValue)
    {
        if (logging)
        {
            if (logging) Debug.Log($"{abilityName}: State changed from {oldValue} to {newValue}");
        }

        //if (newValue<=0) 
        EventHandler();
    }

    protected override void EventHandler()
    {
        //if(logging) Debug.Log($"{abilityName}: Stat TRY Set STATE!!!!!!!!!!!!!!!!!!!!!!!!");
        TryActivateAbility(_character, out _);
    }
}
