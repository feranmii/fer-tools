using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDoorAbility : MonoBehaviour
{
    [ReorderableList] public List<DoorKey> keys;


    public void OpenDoor()
    {
    }

    void UnlockDoor(Door door)
    {
        if (keys.Count <= 0)
        {
            print("You aint got keys");
            return;
        }

        var k = keys.Find(key => key == door.unlockKey);

        if (k != null)
        {
            door.Unlock(k);

            keys.Remove(k);
        }
        else
        {
            print("You aint got the right key");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        var door = other.GetComponent<Door>();

        if (door != null)
        {

            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                if (door.isLocked)
                {
                    UnlockDoor(door);
                }
                else
                {
                    if (!door.isOpen)
                    {
                        door.Open();
                    }
                    else
                    {
                        door.Close();
                    }
                }
            }
        }
    }
    
    
}