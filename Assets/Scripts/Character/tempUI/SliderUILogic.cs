using UnityEngine;
using UnityEngine.UI;

public class SliderUILogic : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private CharacterStatsController statsController;
    [SerializeField] private StatTag _statTag = StatTag.Health;
    [SerializeField] public SceneObjectTag SceneObjectTag;
    [SerializeField] private bool logging = false;

    private void Awake()
    {
        if (_slider == null)
        {
            _slider = GetComponent<Slider>();
            if (_slider == null)
            {
                Debug.LogError("Slider is not assigned and not found on this GameObject", this);
            }
        }

        // Подписываемся на событие инициализации статов
        if (statsController != null)
        {
            SetupSlider();
        }
    }

    private void OnEnable()
    {
        if (SceneObjectTag==SceneObjectTag.Hero)  SelectionManager.OnSelectionChanged += GotNewSelectionAlarm;
        if (SceneObjectTag==SceneObjectTag.Enemy) SelectionManager.OnOpponentChanged += GotNewOppSelectionAlarm;

        if (logging) Debug.Log("HealthBarUI:  Subscribe to OnSelectionChanged is on");
    }

    private void OnDisable()
    {
        //SelectionManager.OnSelectionChanged -= GotNewSelectionAlarm;
        //Debug.Log("HealthBarUI:  UnSubscribe to OnSelectionChanged");
    }

    private void OnDestroy()
    {
        // Важно отписаться от событий при уничтожении объекта
        if (statsController != null && statsController.Stats.TryGetValue(_statTag, out var charStat))
        {
            charStat.OnValueChanged -= OnStatChanged;
            //healthStat.OnBelowZero -= OnHealthBelowZero;
        }

        if (SceneObjectTag == SceneObjectTag.Hero) SelectionManager.OnSelectionChanged -= GotNewSelectionAlarm;
        if (SceneObjectTag == SceneObjectTag.Enemy) SelectionManager.OnOpponentChanged -= GotNewOppSelectionAlarm;
    }

    private void GotNewSelectionAlarm()
    {
        if (logging) Debug.Log("HealthBarUI:  Got invoke - start cheking");
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

    private void GotNewOppSelectionAlarm()
    {
        if (logging) Debug.Log("BarUI:  Got invoke - start cheking");
        if (SelectionManager.HasOpponent)
        {
            if (SelectionManager.OpponentObject.TryGetComponent<CharacterStatsController>(out var newSstatsController))
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
        if (statsController != null && statsController.Stats.TryGetValue(_statTag, out var oldCharStat))
        {
            oldCharStat.OnValueChanged -= OnStatChanged;
            oldCharStat.OnBelowZero -= OnStatBelowZero;
        }

        // 2. Обновляем ссылку на новый контроллер
        statsController = newController;

        // 3. Подписываемся на нового (если он есть)
        if (statsController != null && statsController.Stats.TryGetValue(_statTag, out var newCharStat))
        {
            newCharStat.OnValueChanged += OnStatChanged;
            newCharStat.OnBelowZero += OnStatBelowZero;

            // Обновляем Slider сразу, а не ждём изменения
            _slider.maxValue = newCharStat.BaseValue;
            _slider.value = newCharStat.Value;
        }
        else
        {
            if (logging) Debug.LogWarning($"New StatsController doesn't have {_statTag.ToString()} stat", this);
        }
    }

    private void SetupSlider()
    {
        if (statsController.Stats.TryGetValue(_statTag, out var charStat))
        {
            // Инициализируем Slider
            _slider.minValue = 0;
            _slider.maxValue = charStat.BaseValue;
            _slider.value = charStat.Value;

            // Подписываемся на изменение здоровья
            charStat.OnValueChanged += OnStatChanged;

            // Подписываемся на событие "ниже нуля", если нужно
            charStat.OnBelowZero += OnStatBelowZero;
        }
        else
        {
            if (logging) Debug.LogWarning("StatsController doesn't have Health stat", this);
        }
    }

    private void OnStatChanged(IStat stat, float oldValue, float newValue)
    {
        _slider.value = newValue;

        // Если нужно динамически изменять maxValue (например, при увеличении максимального здоровья)
        // healthSlider.maxValue = stat.BaseValue;
    }

    private void OnStatBelowZero(IStat stat, float value)
    {
        if (logging) Debug.Log("Stt is below zero!");
        // Здесь можно добавить логику смерти персонажа
        //понаблюдать - возможно не нужно
    }


}