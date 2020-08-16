using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public abstract class Collectible : MonoBehaviour
{
    //TODO: Collectible timer.
    [Foldout("Behaviour")] public bool moveToTarget;

    [Foldout("Behaviour"), ShowIf("moveToTarget")]
    public bool moveOnlyWhenInRange;

    [Foldout("Properties"), ShowIf("moveToTarget")]
    public float moveSpeed;

    [Foldout("Properties"), ShowIf("moveOnlyWhenInRange")]
    public float moveRange;

    public Transform target;

    [Tag] public string tag;

    private bool canMove;

    [Foldout("Gizmos")] public bool showGizmos;

    private void Update()
    {
        if (moveToTarget)
        {
            if (moveOnlyWhenInRange)
            {
                if (Vector3.Distance(transform.position, target.position) < moveRange)
                {
                    canMove = true;
                }
            }
            else
            {
                canMove = true;
            }

            if (canMove)
                transform.position =
                    Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(tag)) return;

        Collect();

        gameObject.SetActive(false);
    }

    public abstract void Collect();

    private void OnDrawGizmosSelected()
    {
        if (!showGizmos)
            return;

        
        var c = Colors.SeaGreen;
        c.a = .45f;
        Handles.color = c;
        Handles.DrawSolidDisc(transform.position, transform.up, moveRange);
    }
}