using System;
using System.Collections.Generic;
using EventCallbacks;
using Fertools.Time;
using NaughtyAttributes;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Fertools.Time
{
    public class TimeManager : MonoBehaviour
    {
        public float NormalTimeScale = 1f;

        [ReadOnly] public float CurrentTimeScale = 1f;
        
        // The Time Scale the system is lerping towards
        [ReadOnly] public float TargetTimeScale = 1f;
        
        //If the timescale should be lerping
        [ReadOnly] public bool LerpTimeScale = true;
        
        //Lerping speed of the timescale towards its target
        [ReadOnly] public float LerpSpeed;

        protected Stack<TimeScaleProperties> _timeScaleProperties;
        protected TimeScaleProperties _currentTimeScaleProperty;
        
        private void Start()
        {
            TargetTimeScale = NormalTimeScale;
            _timeScaleProperties = new Stack<TimeScaleProperties>();
            
        }

        private void OnEnable()
        {
            TimeScaleEvent.RegisterListener(TimeScaleEventTrigger);
        }

        private void OnDisable()
        {
            TimeScaleEvent.UnregisterListener(TimeScaleEventTrigger);
        }

        
        public void TimeScaleEventTrigger(TimeScaleEvent tse)
        {
            //_currentTimeScaleProperty = tse.TimeScaleProperty;
            SetTimeScale(tse.TimeScaleProperty);
        }

        [Button()]
        public void TestButtonSlowDown()
        {
            TimeScaleEvent tse = new TimeScaleEvent(0.5f, 3f, true, 3f, false);
            tse.FireEvent();
        }

        private void Update()
        {
            if (_timeScaleProperties.Count > 0)
            {
                _currentTimeScaleProperty = _timeScaleProperties.Peek();
                TargetTimeScale = _currentTimeScaleProperty.TimeScale;
                LerpSpeed = _currentTimeScaleProperty.LerpSpeed;
                LerpTimeScale = _currentTimeScaleProperty.Lerp;
                _currentTimeScaleProperty.Duration -= UnityEngine.Time.unscaledDeltaTime;

                _timeScaleProperties.Pop();
                _timeScaleProperties.Push(_currentTimeScaleProperty);

                if (_currentTimeScaleProperty.Duration <= 0 && !_currentTimeScaleProperty.Infinite)
                {
                    UnFreeze();
                }

            }
            else
            {
                TargetTimeScale = NormalTimeScale;
            }
            
            //Apply the Timescale
            if (LerpTimeScale)
            {
                UnityEngine.Time.timeScale = Mathf.Lerp(UnityEngine.Time.timeScale, TargetTimeScale, UnityEngine.Time.unscaledDeltaTime * LerpSpeed);
            }
            else
            {
                UnityEngine.Time.timeScale = TargetTimeScale;
            }

            CurrentTimeScale = UnityEngine.Time.timeScale;
        }

        public void SetTimeScale(TimeScaleProperties timeScaleProperties)
        {
            _timeScaleProperties.Push(timeScaleProperties);
        }

        public void SetTimeScale(float newTimeScale)
        {
            _timeScaleProperties.Clear();
            
            UnityEngine.Time.timeScale = newTimeScale;
        }

        private void UnFreeze()
        {
            if (_timeScaleProperties.Count > 0)
            {
                _timeScaleProperties.Pop();
            }

            else
            {
                ResetTimeScale();
            }
        }

        private void ResetTimeScale()
        {
            
            UnityEngine.Time.timeScale = NormalTimeScale;
        }
    }
    
    
    
    public struct TimeScaleProperties
    {
        public float TimeScale;
        public float Duration;
        public float LerpSpeed;
        public bool Lerp;
        public bool Infinite;
        
    }
    
    
    
}

#region Time Events 

public class TimeScaleEvent : Event<TimeScaleEvent>
{
    public TimeScaleProperties TimeScaleProperty;

    public TimeScaleEvent(float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite)
    {
        TimeScaleProperty.TimeScale = timeScale;
        TimeScaleProperty.Duration = duration;
        TimeScaleProperty.Lerp = lerp;
        TimeScaleProperty.LerpSpeed = lerpSpeed;
        TimeScaleProperty.Infinite = infinite;
    }        
        
}

public class FixedTimeScaleEvent : Event<FixedTimeScaleEvent>
{
    public TimeScaleProperties TimeScaleProperty;

    public FixedTimeScaleEvent(float timeScale, float duration, bool lerp, float lerpSpeed, bool infinite)
    {
        TimeScaleProperty.TimeScale = timeScale;
        TimeScaleProperty.Duration = duration;
        TimeScaleProperty.Lerp = lerp;
        TimeScaleProperty.LerpSpeed = lerpSpeed;
        TimeScaleProperty.Infinite = infinite;
    }        
        
}

public class OnTimePaused : Event<OnTimePaused>
{
        
}

public class OnTimeResume : Event<OnTimeResume>
{
        
}
    
#endregion
