using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RadialStatBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private StatTag _statTag;
    //[SerializeField] private Canvas _canvas;

    [SerializeField]
    private Color barColor = Color.red; // это поле будет видно в инспекторе

    private IStat stat;
    private IStatsController _statsController;
    private float maxValue = 100f;

    private void Start()
    {
        if (fillImage != null)
        {
            fillImage.color = barColor; // применяем цвет к Image из инспектора
        }

        Initialize();
    }

    public void Initialize()
    {

        _statsController = GetComponentInParent<IStatsController>();//?.GetStatsController();
        if (_statsController == null)
        {
            Debug.LogError("StatsController not found on parent Character");
        }

        if (_statsController == null) return;
        //Debug.LogWarning($" found stat START INITIALIZE");

        if (_statsController.Stats.TryGetValue(_statTag, out var foundStat))
        {
            //if (stat != null)
            //    stat.OnValueChanged -= OnStatChanged;

            stat = foundStat;
            //Debug.LogWarning($" found stat {stat.Name}");
            stat.OnValueChanged += OnStatChanged;

            maxValue = stat.BaseValue;
            UpdateFill(stat.Value, maxValue);
        }
        else
        {
            Debug.LogError($"Stat {_statTag} not found on StatsController");
        }

        UIDisplayManager.OnDisplayingStateChanged += HandleDisplayStateChanged;
    }

    private void OnDestroy()
    {
        if (stat != null)
        {
            stat.OnValueChanged -= OnStatChanged;
        }
        UIDisplayManager.OnDisplayingStateChanged -= HandleDisplayStateChanged;
    }

    private void OnStatChanged(IStat stat, float oldValue, float newValue)
    {
        UpdateFill(newValue, stat.BaseValue);
    }

    private void UpdateFill(float current, float max)
    {
        if (fillImage != null && max > 0)
        {
            fillImage.fillAmount = Mathf.Clamp01(current / max);
        }
    }

    private void HandleDisplayStateChanged(bool isDisplayed)
    {
        fillImage.enabled = isDisplayed;
        //_canvas.enabled = isDisplayed;
    }
}
