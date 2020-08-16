using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Fertools.Pooling;
using TMPro;
using UnityEngine;

public class TextPopup : PooledMonobehaviour
{
    public TextMeshPro textMesh;
    private static int sortingOrder;

    private Sequence _sequence;

    private void Start()
    {
       
    }

    public void Show(TextPopParams popParams)
    {
        transform.position = popParams.target.position;
        textMesh.SetText(popParams.text);
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        textMesh.color = popParams.color;
    }
}

[System.Serializable]
public class TextPopParams
{
    public TextPopParams(Transform target, string text, Color color)
    {
        this.target = target;
        this.text = text;
        this.color = color;
    }

    public Transform target;
    public string text;
    public Color color;
}