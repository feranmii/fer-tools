using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public ScoreVariable scoreVariable;
    public ScoreTable scoreTable;

    private void OnEnable()
    {
        OnPointScored.RegisterListener(HandlePointScored);
        OnStreakScored.RegisterListener(HandleStreakScored);
    }

    private void OnDisable()
    {
        
        OnPointScored.UnregisterListener(HandlePointScored);
        OnStreakScored.UnregisterListener(HandleStreakScored);
    }


    private void HandlePointScored(OnPointScored pointScored)
    {
        scoreVariable.AddScore(scoreTable.scorePoint);
    }

    private void HandleStreakScored(OnStreakScored streakScored)
    {
        scoreVariable.AddScore(scoreTable.streakScore);
    }
    
}

public class OnPointScored : Event<OnPointScored>
{
    
}


public class OnStreakScored : Event<OnStreakScored>
{
    
}


