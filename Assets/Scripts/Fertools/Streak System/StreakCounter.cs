using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class StreakCounter : MonoBehaviour
{
    [Foldout("Streak Info")] public float streakDuration = 5f;
    
    [Foldout("Streak Info"), Header("References")]
    public IntVariable currentStreak;

    [Foldout("Streak Info")] public FloatVariable streakTimer;


    public Action onStreakStart;
    public Action onStreakReset;


    private void Update()
    {
        if (Input.anyKeyDown)
        {
            IncreaseStreak();
        }

        if (!CanStartStreak()) return;

        streakTimer.Value -= Time.deltaTime;

        if (streakTimer.Value <= 0)
        {
            ResetStreak();
            streakTimer.Value = 0;
        }
    }


    private void IncreaseStreak()
    {
        if (currentStreak.Value == 1)
        {
            onStreakStart?.Invoke();
        }

        currentStreak.Value++;

        if (CanStartStreak())
        {
            streakTimer.Value = streakDuration;
        }
    }

    private void ResetStreak()
    {
        currentStreak.Value = 0;
        onStreakReset?.Invoke();
    }

    private bool CanStartStreak()
    {
        return currentStreak.Value >= 0;
    }
}