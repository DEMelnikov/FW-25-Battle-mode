// Расширенный enum с категориями и значениями по умолчанию
using System;
using UnityEngine;
public enum ConstantType
{
    [ConstantInfo("DC", "Default Attack DC", 50f)]
    DC_attack_Default,

    // Player
    [ConstantInfo("Player", "Limit to stop Defence Actions", 5f)]
    DefenceEnergyLimit,

    [ConstantInfo("Player", "Default price to defence roll", 0.5f)]
    DefenceEnergyCost,

    [ConstantInfo("DC", "Default DC", 50f)]
    DefaultDC,

    [ConstantInfo("DC", "Bonus to DC when get Success on Defence ABility check", 7f)]
    SeccessDefenceCheckBonus

    //[ConstantInfo("Player", "Сила прыжка", 10f)]
    //JumpForce,

    //[ConstantInfo("Player", "Максимальное здоровье", 100f)]
    //PlayerMaxHealth,

    //// Physics
    //[ConstantInfo("Physics", "Гравитация", 9.81f)]
    //Gravity,

    //[ConstantInfo("Physics", "Сила трения", 0.2f)]
    //Friction,

    //// Game
    //[ConstantInfo("Game", "Длительность игры (секунды)", 180f)]
    //GameDuration,

    //[ConstantInfo("Game", "Максимальный счет", 1000f)]
    //MaxScore,

    //[ConstantInfo("Game", "Время возрождения", 3f)]
    //RespawnTime,

    //// UI
    //[ConstantInfo("UI", "Длительность анимации UI", 0.3f)]
    //UIAnimationDuration,

    //[ConstantInfo("UI", "Скорость всплывающего текста", 2f)]
    //FloatingTextSpeed
}

// Атрибут для мета-информации
public class ConstantInfoAttribute : PropertyAttribute
{
    public string Category { get; }
    public string Description { get; }
    public float DefaultValue { get; }

    public ConstantInfoAttribute(string category, string description, float defaultValue = 0f)
    {
        Category = category;
        Description = description;
        DefaultValue = defaultValue;
    }
}