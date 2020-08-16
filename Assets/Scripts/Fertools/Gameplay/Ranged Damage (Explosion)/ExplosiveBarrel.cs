using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class ExplosiveBarrel : BaseAreaDamager
{
    private Damageable _damageable;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.OnDeath.AddListener(Explode);
    }
}
