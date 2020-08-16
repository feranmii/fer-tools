using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class GenericCollision : BaseCollisionTrigger
{
    private void OnCollisionEnter(Collision other)
    {
        onEnter.Invoke(other.gameObject);
        onEnterAction?.Invoke(other.gameObject);

        if (destroyOnCollision)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        onStay.Invoke(other.gameObject);
        onStayAction?.Invoke(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        onExit.Invoke(other.gameObject);
        onExitAction?.Invoke(other.gameObject);
    }
}