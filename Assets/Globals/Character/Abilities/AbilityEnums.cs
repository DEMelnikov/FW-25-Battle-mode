using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    Active,
    Passive,
    Toggle
}

public enum AbilityState
{
    Ready,
    Cooldown,
    Active,
    Disabled
}