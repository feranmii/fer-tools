using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Inventory;
using Fertools.Pooling;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TestPoolObject prefab;
    public SecondPoolObject prefabz;


    public WeaponItem currentWeapon;

    private void Start()
    {
        currentWeapon = (WeaponItem) Inventory.Instance.GetItem("AK") ;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Pool.Get<TestPoolObject>();
            prefab.Get<TestPoolObject>(true);
            /*PooledMonobehaviour obj = pool.Get<TestPoolObject>() ;
            obj.gameObject.SetActive(true);*/

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //prefabz.Get<SecondPoolObject>();
            if(currentWeapon != null)
                currentWeapon.Use();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnXPGained gained = new OnXPGained(150);
            gained.FireEvent();
        }
        
    }
}
