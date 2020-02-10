using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Time;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float time = 5000;
    private float t;
    public void Update()
    {
        t += Time.deltaTime;

        if (t >= 1)
        {
            time--;
            t = 0;
        }
    }

    public void Slowmo()
    {
        
    }

    public void Pause()
    {
        
    }
    
}
