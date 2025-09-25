using UnityEngine;

public static class UIDisplayManager
{
    private static bool _isDisplaying = true;

    // �������, ���������� ��� ��������� ��������� �����
    public delegate void DisplayingStateChanged(bool isDisplaying);
    public static event DisplayingStateChanged OnDisplayingStateChanged;

    public static bool IsDisplaying
    {
        get { return _isDisplaying; }
        set
        {
            if (_isDisplaying != value) // ���� ��������� ������������� ����������
            {
                _isDisplaying = value;
                Debug.LogWarning($" Making Invoke STATUS IS {_isDisplaying}");
                OnDisplayingStateChanged?.Invoke(_isDisplaying); // �������� �������
            }
        }
    }

    public static void ToggleDisplaying()
    {
        IsDisplaying = !_isDisplaying; // ���������� ��������, ����� ������������� ������� �������
        //Debug.LogWarning($" DISPLAY STATUS IS {_isDisplaying}");
    }
}