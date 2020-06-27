using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Time;
using UnityEngine;

namespace Fertools.Time
{


    public class FGameTimer : MonoBehaviour
    {

        public TimerController timer;

        [Header("Time Output"), Space(10)] public StringVariable outputTimeString;

        public bool displayHours = true;
        public bool displayMinutes = true;
        public bool displaySeconds = true;


        private void Start()
        {
            timer.Init();
            timer.onTimerEndEvent += () => { print("Teaaah"); };
            UpdateOutput();
        }

        private void Update()
        {
            if (timer == null)
                return;

            timer.Update();
            UpdateOutput();
        }



        private void UpdateOutput()
        {
            if (timer == null)
                return;

            if (outputTimeString != null)
                outputTimeString.Value =
                    FTimeConverter.FloatToTime(timer.Value, displayHours, displayMinutes, displaySeconds);


        }


    }
}

