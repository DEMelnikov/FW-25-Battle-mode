using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.Rendering.STP;

public class CharacterStatsController : MonoBehaviour, IStatsController
{
    public Dictionary<StatTag, IStat> Stats { get; private set; } = new Dictionary<StatTag, IStat>();

    private void Awake()
    {
    }

    private void Update()
    {
        UpdateAllTimedModifiers();
    }

    private void InitializeStats()
    {
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
                var newStat = new Stat(statDef.Name, statDef.Tag, statDef.BaseValue, statDef.HasAlarm, statDef.hasBaseValueLimit);
                newStat.OnValueChanged += HandleStatChanged;
                Stats.Add(statDef.Tag, newStat);
            }
            else
            {
                Debug.LogWarning($"{this.gameObject.name} - Duplicate StatTag {statDef.Tag} in config!", this);
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
        Debug.Log($"[{Time.time}] {this.gameObject.name} -  Stat {stat.Name} changed from {oldValue} to {newValue}");
    }

    public void LogingData()
    {
        foreach (var stat in Stats) 
        {
            Debug.Log($"Stat {stat.Key} = {stat.Value.Value}");
        }
    }

    public void AddTempModifier()
    {
        Stats[StatTag.Strength].AddToTmpModifier(3f);
    }

    // Метод для безопасного получения стата
    public IStat GetStat(StatTag tag)
    {
        if (Stats.ContainsKey(tag))
        {
            return Stats[tag];
        }
        Debug.LogWarning($"Stat with tag {tag} not found!");
        return null;
    }
}



