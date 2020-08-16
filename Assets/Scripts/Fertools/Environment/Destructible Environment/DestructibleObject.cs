using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class DestructibleObject : MonoBehaviour
{
    private Damageable _damageable;


    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }
}
