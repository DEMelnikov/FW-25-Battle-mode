using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsController : MonoBehaviour
{
    public Dictionary<StatTag, Stat> Stats { get; private set; } = new Dictionary<StatTag, Stat>();

    private void Awake()
    {
        //InitializeStats();
    }

    private void Update()
    {
        UpdateAllTimedModifiers();
        //Debug.Log($"Stat {Stats[StatTag.Strength].Name} = {Stats[StatTag.Strength].Value} {Stats[StatTag.Strength].Tag}");

       // Debug.Log( Stats["Strength"].Value);
    }

    private void InitializeStats()
    {
        //Stats.Add(StatTag.Health, new CharacterStat("Health", StatTag.Health, 100f));
        //Stats.Add(StatTag.Mana, new CharacterStat("Mana", StatTag.Mana, 50f));
        Stats.Add(StatTag.Strength, new Stat("Strength", StatTag.Strength, 5f,true));
        //Stats.Add(StatTag.MovementSpeed, new CharacterStat("Movement Speed", StatTag.MovementSpeed, 5f));

        foreach (var stat in Stats.Values)
        {
            stat.OnValueChanged += HandleStatChanged;
        }
    }

    public void SOIntializeStats(SO_CharacterStats enterStats)
    {
        Stats.Add(enterStats.TagSTR, new Stat(enterStats.NameSTR, enterStats.TagSTR, enterStats.BaseValueSTR, enterStats.AlarmBZ_STR));
        Stats.Add(enterStats.TagHP, new Stat(enterStats.NameHP, enterStats.TagHP, enterStats.BaseValueHP, enterStats.AlarmBZ_HP));
        Stats.Add(enterStats.TagEP, new Stat(enterStats.NameEP, enterStats.TagEP, enterStats.BaseValueEP, enterStats.AlarmBZ_EP));

        foreach (var stat in Stats.Values)
        {
            stat.OnValueChanged += HandleStatChanged;
        }
    }

    private void UpdateAllTimedModifiers()
    {
        foreach (var stat in Stats.Values)
        {
            stat.UpdateTimedModifiers(Time.deltaTime);
        }
    }

    private void HandleStatChanged(Stat stat, float oldValue, float newValue)
    {
        Debug.Log($"[{Time.time}] Stat {stat.Name} changed from {oldValue} to {newValue}");
    }

    public void ApplyTemporaryShield()
    {
        var shieldMod = new TimedStatModifier(
            "shield_" + Time.time,
            "Magic Shield",
            StatTag.Strength,
            25f, // +25 к броне
            StatModType.Flat,
            10f); // 20 секунд действия

        Stats[StatTag.Strength].AddTimedModifier(shieldMod);
        Debug.Log("Shield activated!");
    }

    public void LogingData()
    {
        Debug.Log($"Stat {Stats[StatTag.Strength].Name} = {Stats[StatTag.Strength].Value} {Stats[StatTag.Strength].Tag}");
    }

    public void AddTempModifier()
    {
        Stats[StatTag.Strength].AddToTmpModifier(3f);
    }
}



