using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay/Input/Player Input")]
public class PlayerInput : ScriptableObject
{
    [InfoBox("If enabled, The movement input won't be smoothed.")]
    public bool useRawMovement;
    [ReadOnly] public Vector3 moveInputVector;
    [ReadOnly] public Vector3 normalizedMoveInputVector;
    [ReadOnly] public bool isMoving;
    
    public void Update()
    {
        moveInputVector.x = useRawMovement ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal") ;
        moveInputVector.z = useRawMovement ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");
        normalizedMoveInputVector = moveInputVector.normalized;

        isMoving = normalizedMoveInputVector.magnitude > 0.01;
    }
}
