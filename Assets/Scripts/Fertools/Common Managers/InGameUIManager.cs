using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class InGameUIManager : UIScreenManager
{
    //TODO: Take this to level config file
    [Space] public float winLoseScreenDelay = 1.5f;

    [Foldout("Non-Button Controlled Screens")]
    public GameObject winScreen;

    [Foldout("Non-Button Controlled Screens")]
    public GameObject gameOverScreen;

    private void OnEnable()
    {
        LevelWinEvent.RegisterListener(ShowWinScreen);
        LevelLoseEvent.RegisterListener(ShowLoseScreen);
    }

    private void OnDisable()
    {
        LevelWinEvent.UnregisterListener(ShowWinScreen);
        LevelLoseEvent.UnregisterListener(ShowLoseScreen);
    }

    private void ShowWinScreen(LevelWinEvent level)
    {
        OpenScreen(winScreen);
    }

    private void ShowLoseScreen(LevelLoseEvent level)
    {
        OpenScreen(gameOverScreen);
    }
}