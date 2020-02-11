using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Pooling;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TestPoolObject prefab;
    public SecondPoolObject prefabz;
    
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
            prefabz.Get<SecondPoolObject>();
        }
        
    }
}
