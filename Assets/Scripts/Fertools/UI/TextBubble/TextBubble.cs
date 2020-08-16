using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBubble : MonoBehaviour
{
    public bool alwaysFaceCamera;

    [Foldout("Speed")] public float openSpeed = .3f;
    [Foldout("Speed")] public float textFadeSpeed = .15f;
    [Foldout("References")] public TextMeshProUGUI tmp;

    [TextArea] public string targetText;
    private Tweener _tweener;
    private Tweener _textTweener;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;

        _textTweener = tmp.DOFade(1, textFadeSpeed).SetAutoKill(false).Pause();
        _tweener = transform.DOScale(Vector3.zero, openSpeed)
            .From()
            .SetEase(Ease.OutBack)
            .SetAutoKill(false)
            .Pause()
            .OnComplete(() => { _textTweener.Restart(); });
    }

    public void SetText(string text)
    {
        targetText = text;
        tmp.text = text;

        var c = new Color(0, 0, 0, 0);
        tmp.color = c;


        _tweener.Restart();
    }

    void LateUpdate()
    {
        if (alwaysFaceCamera)
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);
        }
    }


    [Button()]
    public void Test()
    {
        SetText(targetText);
    }
}