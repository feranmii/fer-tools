using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject
{
    public bool isSerialiazable = false;
    public float value;

    private void OnEnable()
    {
        if(!isSerialiazable)
            value = 0;
    }
}
