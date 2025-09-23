using System;
using UnityEngine;
using UnityEngine.AI;

public class TimerTrigger
{
    private float _duration;
    private float _currentTime;
    private bool  _isRunning;
    private bool  _isGlobalPause;
    private bool  _isLooped;
    private int   _maxLoops;
    private int   _loopsCompleted;
    private bool  _isSubscribed = false;

    // Коллбэки вместо событий
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
        _duration = duration;
        _onTick = onTick;
        _onStart = onStart;
        _onComplete = onComplete;
        _isLooped = looped;
        _maxLoops = maxLoops;
        SubscribeToPauseEvents();
        _isGlobalPause = PauseManager.IsPaused;
        _isSubscribed = false;
    }

    //public TimerTrigger(
    //float duration, 
    //bool looped,
    //Action onTick = null,
    //Action onStart = null,
    //Action onComplete = null,
    //int maxLoops = -1)
    //{
    //    _duration = duration;
    //    _onTick = onTick;
    //    _onStart = onStart;
    //    _onComplete = onComplete;
    //    _isLooped = looped;
    //    _maxLoops = maxLoops;
    //}

    public void Update(float deltaTime)
    {
        if (!_isRunning || _isGlobalPause) return;

        _currentTime += deltaTime;

        if (_currentTime >= _duration)
        {
            CompleteCycle();
        }
    }

    public void Start()
    {
        _isRunning = true;
        _currentTime = 0f;
        _loopsCompleted = 0;
        _onStart?.Invoke();
    }

    private void CompleteCycle()
    {
        _currentTime = 0f;
        _loopsCompleted++;
        _onTick?.Invoke();

        if (_isLooped && (_maxLoops < 0 || _loopsCompleted < _maxLoops))
        {
            Start();
        }
        else
        {
            _isRunning = false;
            _onComplete?.Invoke();
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }

    /// Сброс таймера

    public void Reset()
    {
        _currentTime = 0f;
        _loopsCompleted = 0;
    }

    public void ResumeTimer()
    {
        _isRunning = true;
    }

    // Свойства для доступа к состоянию
    public bool IsRunning => _isRunning;
    public float Progress => Mathf.Clamp01(_currentTime / _duration);
    public int LoopsCompleted => _loopsCompleted;
    public float RemainingTime => _duration - _currentTime;

    private void SubscribeToPauseEvents()
    {
        if (!_isSubscribed)
        {
            PauseManager.OnPauseStateChanged += HandlePauseStateChanged;
            _isSubscribed = true;
        }
    }
    private void HandlePauseStateChanged(bool isPaused)
    {
        _isGlobalPause = isPaused;
    }
    public void UnsubscribeFromPauseEvents()
    {
        if (_isSubscribed)
        {
            PauseManager.OnPauseStateChanged -= HandlePauseStateChanged;
            _isSubscribed = false;
        }
    }
    
    // ...остальные методы (Stop, Reset и свойства) как в предыдущем примере...
}