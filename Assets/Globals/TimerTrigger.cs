using System;
using UnityEngine;

public class TimerTrigger
{
    private float _duration;
    private float _currentTime;
    private bool _isRunning;
    private bool _isLooped;
    private int _maxLoops;         // -1 = ����������
    private int _loopsCompleted;

    // �������� (����������: OnStart � ��� ������, OnTick � ��� ���������� ������� ���������, OnComplete � ��� ������ ����������)
    private Action _onStart;
    private Action _onTick;
    private Action _onComplete;

    public TimerTrigger(
        float duration,
        Action onTick = null,
        Action onStart = null,
        Action onComplete = null,
        bool looped = false,
        int maxLoops = -1)
    {
        SetDuration(duration);
        _onTick = onTick;
        _onStart = onStart;
        _onComplete = onComplete;
        _isLooped = looped;
        _maxLoops = maxLoops;
    }

    // ���������� ������� ������� deltaTime
    public void Update(float deltaTime)
    {
        if (!_isRunning) return;
        if (deltaTime <= 0f) return;

        _currentTime += deltaTime;

        // ������������ ��� ��������� "��������" �� ���� ����
        while (_isRunning && _currentTime >= _duration)
        {
            _currentTime -= _duration;
            _loopsCompleted++;

            SafeInvoke(_onTick);

            // ��������� ������� ����������
            if (!_isLooped || (_maxLoops >= 0 && _loopsCompleted >= _maxLoops))
            {
                _isRunning = false;
                _currentTime = 0f; // ����� �������� �������, ���� ����� "������" ���������
                SafeInvoke(_onComplete);
                break;
            }
        }
    }

    // �����. �� ��������� ��� ������ ��������� (����� ����� ���� "Resume" ����� Start()).
    // ���� ����� ������ ������� � �������� Restart().
    public void Start(bool reset = false)
    {
        if (reset)
        {
            _currentTime = 0f;
            _loopsCompleted = 0;
        }

        if (_isRunning) return;

        _isRunning = true;
        SafeInvoke(_onStart);
    }

    // ������ ������� � ���������� �����
    public void Restart()
    {
        _currentTime = 0f;
        _loopsCompleted = 0;
        if (!_isRunning)
            SafeInvoke(_onStart);
        _isRunning = true;
    }

    public void Stop() => _isRunning = false;

    // ����� ������� ��� ������
    public void Reset()
    {
        _currentTime = 0f;
        _loopsCompleted = 0;
    }

    public void Pause() => _isRunning = false;
    public void Resume() => _isRunning = true;

    // �������� ������������ �� ����
    public void SetDuration(float duration)
    {
        if (duration <= 0f)
            throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be > 0.");
        _duration = duration;
        // �����������: ��������������� ������� ��������, ���� �� ����� �� ����� ��������
        if (_currentTime > _duration) _currentTime = _duration;
    }

    // ��������
    public bool IsRunning => _isRunning;
    public float Duration => _duration;
    public float ElapsedInCycle => Mathf.Clamp(_currentTime, 0f, _duration);
    public float Progress => _duration > 0f ? Mathf.Clamp01(_currentTime / _duration) : 1f;
    public float RemainingTime => Mathf.Max(0f, _duration - _currentTime);
    public int LoopsCompleted => _loopsCompleted;

    // ������������
    public bool IsLooped
    {
        get => _isLooped;
        set => _isLooped = value;
    }

    // -1 = ����������, 0 = �������� 0 ������ (����� Complete), N > 0 = ������������ �����
    public int MaxLoops
    {
        get => _maxLoops;
        set => _maxLoops = value;
    }

    // ���������/������ ���������
    public void SetCallbacks(Action onTick = null, Action onStart = null, Action onComplete = null)
    {
        _onTick = onTick;
        _onStart = onStart;
        _onComplete = onComplete;
    }

    private static void SafeInvoke(Action action)
    {
        try { action?.Invoke(); }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}