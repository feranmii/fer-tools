using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Weapons.Guns;
using UnityEngine;

public class GunShooting : MonoBehaviour
{
    public Gun currentGun;

    private float _t;

    private void Start()
    {
        
    }

    private void Update()
    {
        _t += Time.deltaTime;

        if ((_t <= currentGun.gunData.fireRate)) return;
        
        if(Input.GetButton("Fire1"))
        {
            currentGun.Use();
            _t = 0;
        }
        

    }
    
    
}
