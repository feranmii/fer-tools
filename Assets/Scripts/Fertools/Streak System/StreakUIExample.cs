using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreakUIExample : MonoBehaviour
{
    public IntVariable streakCount;
    public FloatVariable streakTimer;
    public CanvasGroup streakUI;
    public TextMeshProUGUI streakCountText;
    public Image timer;


    private StreakCounter _streakCounter;

    private Tweener showStreak;
    private Tweener closeStreak;

    private void Awake()
    {
        _streakCounter = FindObjectOfType<StreakCounter>();

        showStreak = streakUI.DOFade(1, .5f).From(0).Pause();

        closeStreak = streakUI.DOFade(0, .5f).OnComplete(() => streakUI.gameObject.SetActive(false)).Pause();
    }

    private void Start()
    {
        _streakCounter.onStreakStart += StartStreak;
        _streakCounter.onStreakReset += ResetStreak;


        ResetStreak();
    }

    private void Update()
    {
        RefreshStreakUI();
    }

    private void StartStreak()
    {
        print("Stasrtogm");

        
        streakUI.gameObject.SetActive(true);
        showStreak.Restart();
    }

    private void ResetStreak()
    {
        print("Reseting");
        closeStreak.Restart();
    }

    private void RefreshStreakUI()
    {
        streakCountText.text = streakCount.Value.ToString();
        timer.fillAmount = streakTimer.Value / _streakCounter.streakDuration;
    }
}