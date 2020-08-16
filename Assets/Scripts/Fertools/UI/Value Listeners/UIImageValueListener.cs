using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIImageValueListener : MonoBehaviour
{

   [Space]
    public Image targetImage;

    public float valueChangeDuration = .25f;
    
    public FloatVariable floatVariable;


    [Space] public UnityEvent onValueChanged;

    //-----------Editor Checks------------
  

    private void OnEnable()
    {
        floatVariable.onValueChanged += OnValueChanged;
    }

    void OnValueChanged<T>(T value)
    {
        if (targetImage != null)
        {
            targetImage.DOFillAmount( floatVariable.Value, valueChangeDuration);
        }
        
        onValueChanged?.Invoke();
        
    }
    

}