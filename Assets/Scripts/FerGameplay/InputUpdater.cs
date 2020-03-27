using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputUpdater : MonoBehaviour
{
    public PlayerInput playerInput;

    private void Update()
    {
        if(playerInput != null)
            playerInput.Update();
        else
            Debug.LogError("Player Input Not Assigned");
        
    }
}
