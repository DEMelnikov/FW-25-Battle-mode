using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CharacterStatsController statsController;

    private void Awake()
    {
        if (healthSlider == null)
        {
            healthSlider = GetComponent<Slider>();
            if (healthSlider == null)
            {
                Debug.LogError("HealthSlider is not assigned and not found on this GameObject", this);
            }
        }

        // Подписываемся на событие инициализации статов
        if (statsController != null)
        {
            SetupHealthSlider();
        }
    }

    private void OnEnable()
    {
        SelectionManager.OnSelectionChanged += GotNewSelectionAlarm;
        Debug.Log("HealthBarUI:  Subscribe to OnSelectionChanged is on");
    }

    private void OnDisable()
    {
        //SelectionManager.OnSelectionChanged -= GotNewSelectionAlarm;
        //Debug.Log("HealthBarUI:  UnSubscribe to OnSelectionChanged");
    }

    private void OnDestroy()
    {
        // Важно отписаться от событий при уничтожении объекта
        if (statsController != null && statsController.Stats.TryGetValue(StatTag.Health, out var healthStat))
        {
            healthStat.OnValueChanged -= OnHealthChanged;
            //healthStat.OnBelowZero -= OnHealthBelowZero;
        }

        SelectionManager.OnSelectionChanged -= GotNewSelectionAlarm;
    }

    private void GotNewSelectionAlarm()
    {
        Debug.Log("HealthBarUI:  Got invoke - start cheking");
        if (SelectionManager.HasSelection)
        {
            if (SelectionManager.SelectedObject.TryGetComponent<CharacterStatsController>(out var newSstatsController))
            { 
                if (newSstatsController != statsController)
                {
                    SetStatsController(newSstatsController);
                    //SetupHealthSlider();
                }
            } 
        }
    }

    public void SetStatsController(CharacterStatsController newController)
    {
        // 1. Отписываемся от старого контроллера (если он был)
        if (statsController != null && statsController.Stats.TryGetValue(StatTag.Health, out var oldHealthStat))
        {
            oldHealthStat.OnValueChanged -= OnHealthChanged;
            oldHealthStat.OnBelowZero -= OnHealthBelowZero;
        }

        // 2. Обновляем ссылку на новый контроллер
        statsController = newController;

        // 3. Подписываемся на нового (если он есть)
        if (statsController != null && statsController.Stats.TryGetValue(StatTag.Health, out var newHealthStat))
        {
            newHealthStat.OnValueChanged += OnHealthChanged;
            newHealthStat.OnBelowZero += OnHealthBelowZero;

            // Обновляем Slider сразу, а не ждём изменения
            healthSlider.maxValue = newHealthStat.BaseValue;
            healthSlider.value = newHealthStat.Value;
        }
        else
        {
            Debug.LogWarning("New StatsController doesn't have Health stat", this);
        }
    }

    private void SetupHealthSlider()
    {
        if (statsController.Stats.TryGetValue(StatTag.Health, out var healthStat))
        {
            // Инициализируем Slider
            healthSlider.minValue = 0;
            healthSlider.maxValue = healthStat.BaseValue;
            healthSlider.value = healthStat.Value;

            // Подписываемся на изменение здоровья
            healthStat.OnValueChanged += OnHealthChanged;

            // Подписываемся на событие "ниже нуля", если нужно
            healthStat.OnBelowZero += OnHealthBelowZero;
        }
        else
        {
            Debug.LogWarning("StatsController doesn't have Health stat", this);
        }
    }

    private void OnHealthChanged(Stat stat, float oldValue, float newValue)
    {
        healthSlider.value = newValue;

        // Если нужно динамически изменять maxValue (например, при увеличении максимального здоровья)
        // healthSlider.maxValue = stat.BaseValue;
    }

    private void OnHealthBelowZero(Stat stat, float value)
    {
        Debug.Log("Health is below zero!");
        // Здесь можно добавить логику смерти персонажа
    }


}