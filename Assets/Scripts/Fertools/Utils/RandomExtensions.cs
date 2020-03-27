using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;

public static class RandomExtensions 
{
    public static bool RandomBool()
    {
        var retVal = false;
        
        var rand = new Random();
        var val = rand.Next(0, 2);
        
        if(val == 1)
        {
            retVal = true;
        }

        return retVal;
    }
}
