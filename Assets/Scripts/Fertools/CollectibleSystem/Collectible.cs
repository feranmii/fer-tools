using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    //TODO: Collectible timer.


    [Tag] public string tag;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tag)) return;

        Collect();
        
        gameObject.SetActive(false);
    }
    
    public abstract void Collect();

}
