using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

public class BaseCollisionTrigger : MonoBehaviour
{
    [Foldout("Behaviour")] public bool destroyOnCollision;

    [Foldout("Behaviour"), ShowIf("destroyOnCollision")]
    public float destroyDelay = 0;


    [Space] [Foldout("Events")] public CollisionEvent onEnter;
    [Foldout("Events")] public CollisionEvent onStay;
    [Foldout("Events")] public CollisionEvent onExit;

    //To handle collision through code
    
    public Action<GameObject> onEnterAction;
    public Action<GameObject> onStayAction;
    public Action<GameObject> onExitAction;
  
}

[System.Serializable]
public class CollisionEvent : UnityEvent<GameObject>
{
}