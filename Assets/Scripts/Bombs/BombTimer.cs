using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BombTimer : MonoBehaviour
{
    private Dictionary<UnityEvent, float> _timeEvents;
    private float _timeCount;
    private float _nextEventTime;
    private UnityEvent _nextEvent;
    private UnityAction _finalCallback;
    private bool _timerEnabled;

    public void SetupTimer(float time, UnityAction callback, Dictionary<UnityEvent, float> timeActions) 
    {
        _timerEnabled = true;
        _timeCount = time;

        _timeEvents = new Dictionary<UnityEvent, float>();
        _timeEvents = timeActions;

        _nextEventTime = GetNextActionTime();

        _finalCallback = callback;
    }

    public void StopTimer() 
    {
        _timerEnabled = false;
        _timeEvents = new Dictionary<UnityEvent, float>();
    }

    private void Update()
    {
        if (!_timerEnabled)
            return;

        _timeCount -= Time.deltaTime;
        if(_timeCount <= _nextEventTime) 
        {
            _nextEvent?.Invoke();
            _nextEventTime = GetNextActionTime();
        }

        if (_timeCount <= 0) 
        {
            _timerEnabled = false;
            _timeCount = 0;

            _finalCallback?.Invoke();
        }
    }

    private float GetNextActionTime() 
    {
        float nextActionTime = -1f;

        foreach (UnityEvent key in _timeEvents.Keys)
        {
            nextActionTime = _timeEvents[key];
            _nextEvent = key;
            _timeEvents.Remove(key);
            break;
        }

        return nextActionTime;
    }
}
