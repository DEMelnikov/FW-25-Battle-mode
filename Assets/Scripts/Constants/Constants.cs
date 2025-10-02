using UnityEngine;
using System.Collections.Generic;

public static class Constants
{
    private static GameConstants _instance;
    private static bool _initialized = false;

    public static GameConstants Instance
    {
        get
        {
            if (!_initialized)
                Initialize();
            return _instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        if (_initialized) return;

        _instance = Resources.Load<GameConstants>("GameConstants");

        if (_instance != null)
        {
            _instance.Initialize();
            Debug.Log("GameConstants loaded successfully");
        }
        else
        {
            Debug.LogError("GameConstants asset not found in Resources folder!");
        }

        _initialized = true;
    }

    // Быстрый доступ к константам
    public static float Get(ConstantType type)
    {
        return Instance?.GetValue(type) ?? 0f;
    }

    // Специфичные свойства для удобства
    public static float attackDC => Get(ConstantType.DC_attack_Default);

    //public static float PlayerSpeed => Get(ConstantType.PlayerSpeed);
    //public static float JumpForce => Get(ConstantType.JumpForce);
    //public static float Gravity => Get(ConstantType.Gravity);
    //public static float PlayerMaxHealth => Get(ConstantType.PlayerMaxHealth);
}