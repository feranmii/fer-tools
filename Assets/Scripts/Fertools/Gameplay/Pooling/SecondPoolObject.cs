using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Pooling;
using UnityEngine;

public class SecondPoolObject : PooledMonobehaviour
{
    private void OnEnable()
    {
        Invoke("Disable", 1);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
