using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.Rendering.STP;

public class CharacterStatsController : MonoBehaviour, IStatsController
{
    public Dictionary<StatTag, IStat> Stats { get; private set; } = new Dictionary<StatTag, IStat>();

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

    public void SOIntializeStats(SO_CharacterStatsConfig config)
    {
        Stats.Clear();

        foreach (var statDef in config.Stats)
        {
            if (!Stats.ContainsKey(statDef.Tag))
            {
                var newStat = new Stat(statDef.Name, statDef.Tag, statDef.BaseValue, statDef.HasAlarm);
                newStat.OnValueChanged += HandleStatChanged;
                Stats.Add(statDef.Tag, newStat);
            }
            else
            {
                Debug.LogWarning($"Duplicate StatTag {statDef.Tag} in config!", this);
            }
        }
    }


    private void UpdateAllTimedModifiers()
    {
        foreach (var stat in Stats.Values)
        {
            stat.UpdateTimedModifiers(Time.deltaTime);
        }
    }

    private void HandleStatChanged(IStat stat, float oldValue, float newValue)
    {
        Debug.Log($"[{Time.time}] Stat {stat.Name} changed from {oldValue} to {newValue}");
    }

    public void LogingData()
    {
        //Debug.Log($"Stat {Stats[StatTag.Strength].Name} = {Stats[StatTag.Strength].Value} {Stats[StatTag.Strength].Tag}");
        //Debug.Log($"Stat {Stats[StatTag.Health].Name} = {Stats[StatTag.Health].Value} {Stats[StatTag.Health].Tag}");
        //Debug.Log($"Stat {Stats[StatTag.Energy].Name} = {Stats[StatTag.Energy].Value} {Stats[StatTag.Energy].Tag}");

        foreach (var stat in Stats) 
        {
            Debug.Log($"Stat {stat.Key} = {stat.Value.Value}");
        }
    }

    public void AddTempModifier()
    {
        Stats[StatTag.Strength].AddToTmpModifier(3f);
    }
}



