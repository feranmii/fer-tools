using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Pooling;
using NaughtyAttributes;
using UnityEngine;

public class TextPopExample : MonoBehaviour
{
    public Transform textTarget;

    public TextPopup textPopup;

    private void Update()
    {
        if(Input.anyKeyDown)
            TestPop();
        
    }

    [Button()]
    void TestPop()
    {
        var tp = new TextPopParams(textTarget, "SDsd", Colors.IndianRed);

        textPopup.Get<TextPopup>();
        textPopup.Show(tp);
    }
}