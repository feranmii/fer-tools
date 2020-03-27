using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public PlayerStats stats;
    public PlayerInput playerInput;
    
    private void FixedUpdate()
    {
        if (!playerInput.isMoving)
            return;
        
        Quaternion lookDir = Quaternion.LookRotation(playerInput.normalizedMoveInputVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, stats.rotationSpeed);
    }
    
    
}
