using UnityEngine;

public static class UIDisplayManager
{
    private static bool _isDisplaying = true;

    // Событие, вызываемое при изменении состояния паузы
    public delegate void DisplayingStateChanged(bool isDisplaying);
    public static event DisplayingStateChanged OnDisplayingStateChanged;

    public static bool IsDisplaying
    {
        get { return _isDisplaying; }
        set
        {
            if (_isDisplaying != value) // Если состояние действительно изменилось
            {
                _isDisplaying = value;
                Debug.LogWarning($" Making Invoke STATUS IS {_isDisplaying}");
                OnDisplayingStateChanged?.Invoke(_isDisplaying); // Вызываем событие
            }
        }
    }

    public static void ToggleDisplaying()
    {
        IsDisplaying = !_isDisplaying; // Используем свойство, чтобы автоматически вызвать событие
        //Debug.LogWarning($" DISPLAY STATUS IS {_isDisplaying}");
    }
}