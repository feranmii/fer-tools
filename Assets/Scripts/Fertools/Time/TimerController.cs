using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(menuName = "Time/Timer Controller")]
    public class TimerController : ScriptableObject
    {
        public enum TimerType
        {
            Elapsing,
            Countdown
        }

        public TimerType timerType;

        [Space(10)] public bool beginOnStart = true;
        public int maxTime;

        private int timer;

        public int Value => timer;

        public delegate void TimeDelegate();

        public TimeDelegate onTimerEndEvent;
        public TimeDelegate onTimerStartEvent;
        
        private float _t;
        private bool _paused;
        private bool _hasInit;

        public void Init()
        {
            timer = 0;
            timer = timerType == TimerType.Countdown ? maxTime : 0;
            _paused = !beginOnStart;
            _hasInit = true;
            
            
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Reset();
            }
            
            //TODO: Solve Init Issues

            if (!_hasInit)
                Debug.LogError("Init() has not been called.");

            if (!_paused)
            {
                _t += Time.deltaTime;

                if (_t >= 1)
                {
                    _t = 0;

                    timer += timerType == TimerType.Countdown ? -1 : 1;

                    if (timerType == TimerType.Countdown && timer <= 0 ||
                        timerType == TimerType.Elapsing && timer >= maxTime)
                    {
                        ToggleTimer();
                        OnTimerEnd();
                    }

                }
            }
        }


        public void Reset()
        {
            
            Init();
        }

        public void ToggleTimer()
        {
            _paused = !_paused;
        }


        private void OnTimerEnd()
        {
            onTimerEndEvent?.Invoke();
        }

        private void OnTimerStart()
        {
            
        }
    }
