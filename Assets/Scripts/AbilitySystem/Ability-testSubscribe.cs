using Unity.Android.Gradle;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Ability_subscribe", menuName = "FW25/Ability System/Ability With Stat Subscribe")]
public class AbilityWithSubscribeToStat: Ability
{
    [SerializeField] private StatTag _statToSubscribe = StatTag.Energy;

    public override void Subscribe(ICharacter character)
    {
        base.Subscribe(character);
        if (character == null || IsSubscribed) return;

        if(character.GetStatsController().Stats.TryGetValue(_statToSubscribe, out var foundStat))
        {
            foundStat.OnValueChanged += OnStatValueChanged;
        }

        IsSubscribed = true;
    }

    public override void Unsubscribe(ICharacter character)
    {
        base.Unsubscribe(character);
        if (!IsSubscribed) return;

        if (character.GetStatsController().Stats.TryGetValue(_statToSubscribe, out var foundStat))
        {
            foundStat.OnValueChanged -= OnStatValueChanged;
        }

        IsSubscribed = false;
    }

    private void OnStatValueChanged(IStat stat, float oldValue, float newValue)
    {
        if (logging)
        {
            Debug.Log($"{abilityName}: Stat {stat.Name} changed from {oldValue} to {newValue}");
        }

        EventHandler();
    }

    protected override void EventHandler()
    {
        //Debug.LogWarning("Your SubscribedAbility is works!!!!");
    }
}
