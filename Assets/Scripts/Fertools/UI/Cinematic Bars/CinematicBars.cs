using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventCallbacks;
using NaughtyAttributes;
using UnityEngine;

public class CinematicBars : MonoBehaviour
{
    [Foldout("Info")] [Space] public float distance = 250;
    [Foldout("Info")] [Space] public float duration = .5f;

    [Foldout("References")] public RectTransform topBar;
    [Foldout("References")] public RectTransform bottomBar;

    private Tweener topTweener;
    private Tweener bottomTweener;

    private void Start()
    {
        topTweener = topBar.DOSizeDelta(new Vector2(0, distance), duration).SetAutoKill(false).Pause();
        bottomTweener = bottomBar.DOSizeDelta(new Vector2(0, distance), duration).SetAutoKill(false).Pause();
    }


    private void OnEnable()
    {
        ShowCinematicBarsEvent.RegisterListener(ProcessShowEvent);
        HideCinematicBarsEvent.RegisterListener(ProcessHideEvent);
    }

    private void OnDisable()
    {
        ShowCinematicBarsEvent.UnregisterListener(ProcessShowEvent);
        HideCinematicBarsEvent.UnregisterListener(ProcessHideEvent);
    }

    private void ProcessShowEvent(ShowCinematicBarsEvent res)
    {
        Show();
    }

    private void ProcessHideEvent(HideCinematicBarsEvent res)
    {
        Hide();
    }


    [Button()]
    public void Show()
    {
        topTweener.Restart();
        bottomTweener.Restart();
    }

    [Button()]
    public void Hide()
    {
        topTweener.PlayBackwards();
        bottomTweener.PlayBackwards();
    }
}

public class ShowCinematicBarsEvent : Event<ShowCinematicBarsEvent>
{
}

public class HideCinematicBarsEvent : Event<HideCinematicBarsEvent>
{
}