using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericTrigger : BaseCollisionTrigger
{
    private void OnTriggerEnter(Collider other)
    {
        onEnter.Invoke(other.gameObject);
        onEnterAction?.Invoke(other.gameObject);
        if (destroyOnCollision)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        onStay.Invoke(other.gameObject);
        onStayAction?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        onExit.Invoke(other.gameObject);
        onExitAction?.Invoke(other.gameObject);
    }
}