using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Time;
using UnityEngine;

public class FGameTimer : MonoBehaviour
{
    
    public TimerVariable timer;
    
    [Header("Time Output"), Space(10)]
    public StringVariable outputTimeString;

    public bool displayHours = true;
    public bool displayMinutes = true;
    public bool displaySeconds = true;
    

    private void Start()
    {
        timer.Init();
        UpdateOutput();
    }

    private void Update()
    {
        if(timer == null)
            return;
        
        timer.Update();
        UpdateOutput();
    }

  
  
    private void UpdateOutput()
    {
        if(timer == null)
            return;
        
        if(outputTimeString != null) outputTimeString.value = FTimeConverter.FloatToTime(timer.Value, displayHours, displayMinutes, displaySeconds);

        print(FTimeConverter.FloatToTime(timer.Value, displayHours, displayMinutes, displaySeconds));
        
    }
    
  
}

