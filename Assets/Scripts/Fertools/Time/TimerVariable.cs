using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(menuName = "Time/Timer Variable")]
    public class TimerVariable : ScriptableObject
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
                    }

                }
            }
        }


        public void ToggleTimer()
        {
            _paused = !_paused;
        }


    }
