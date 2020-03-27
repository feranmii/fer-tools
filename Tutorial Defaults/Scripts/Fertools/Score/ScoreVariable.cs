using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreVariable : MonoBehaviour
{
    private int _score;

    public int Value
    {
        get => _score;

        set
        {
            _score = value;
            
            if (_score > LocalStorage.Highscore)
            {
                LocalStorage.Highscore = _score;
            }
        }
    }

    public void AddScore(int value)
    {
        _score += value;
    }

    public void ReduceScore(int value)
    {
        _score -= value;

        if (_score <= 0)
            _score = 0;
        
    }
    
}
