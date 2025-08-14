using UnityEngine;

public static class PauseManager
{
    private static bool _isPaused = true;

    // Событие, вызываемое при изменении состояния паузы
    public delegate void PauseStateChanged(bool isPaused);
    public static event PauseStateChanged OnPauseStateChanged;

    public static bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            if (_isPaused != value) // Если состояние действительно изменилось
            {
                _isPaused = value;
                OnPauseStateChanged?.Invoke(_isPaused); // Вызываем событие
            }
        }
    }

    public static void TogglePause()
    {
        IsPaused = !IsPaused; // Используем свойство, чтобы автоматически вызвать событие
    }
}