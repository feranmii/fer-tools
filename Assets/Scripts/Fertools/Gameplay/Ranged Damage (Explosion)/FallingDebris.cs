using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingDebris : BaseAreaDamager
{
    public LayerMask mask;

    private void OnCollisionEnter(Collision other)
    {
        if ((mask.value & 1 << other.gameObject.layer) != 0)
        {
           Explode();
        }
        else
        {
            print("Its not");
        }
    }
}
