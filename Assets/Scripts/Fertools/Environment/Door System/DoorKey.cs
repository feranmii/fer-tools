using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Key")]
public class DoorKey : ScriptableObject
{
    [Min(1)] public int numberOfUses = 1;

    public void Use()
    {
        // numberOfUses--;
        //
        // numberOfUses = Mathf.Max(0, numberOfUses);
    }
}