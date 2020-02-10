using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fertools.Time
{
    public class FTimeConverter
    {
        public static string FloatToTime(float time, bool showHours = false, bool showMinutes = true, bool showSeconds = false)
        {
            var returnValue = "";

            float hours = Mathf.Floor(time / 3600);
            float v = time % 3600;
            float minutes = Mathf.Floor(v / 60);
            float seconds = Mathf.Floor(v % 60);

            if (showHours && showMinutes && showSeconds)
            {
                returnValue = $"{hours:00}:{minutes:00}:{seconds:00}";
            }

            if (!showHours && showMinutes && showSeconds)
            {
                returnValue = $"{minutes:00}:{seconds:00}";
            }
            
            if (!showHours && showMinutes && !showSeconds)
            {
                returnValue = $"{minutes:00}";
            }
            
            if (showHours && showMinutes && !showSeconds)
            {
                returnValue = $"{hours:00}:{minutes:00}";
            }
            
            
            return returnValue;
        }
         public static TimeValues FloatToTimeObject(float time)
        {
            TimeValues returnValue;

            
            float hours = Mathf.Floor(time / 3600);
            float v = time % 3600;
            float minutes = Mathf.Floor(v / 60);
            float seconds = Mathf.Floor(v % 60);
            
            returnValue = new TimeValues
            {
                convertedHours =  hours,
                convertedMinutes =  minutes,
                convertedSeconds =  seconds
            };
            
            return returnValue;
        }
        
    }
}

public struct TimeValues
{
    public float convertedHours;
    public float convertedMinutes;
    public float convertedSeconds;
}