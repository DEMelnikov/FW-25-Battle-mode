using System.Collections.Generic;
using UnityEngine;

public interface IStatsController
{
    public Dictionary<StatTag, IStat> Stats { get; }
}
