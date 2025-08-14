using UnityEngine;

public static class PauseManager
{
    private static bool _isPaused = true;

    // �������, ���������� ��� ��������� ��������� �����
    public delegate void PauseStateChanged(bool isPaused);
    public static event PauseStateChanged OnPauseStateChanged;

    public static bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            if (_isPaused != value) // ���� ��������� ������������� ����������
            {
                _isPaused = value;
                OnPauseStateChanged?.Invoke(_isPaused); // �������� �������
            }
        }
    }

    public static void TogglePause()
    {
        IsPaused = !IsPaused; // ���������� ��������, ����� ������������� ������� �������
    }
}