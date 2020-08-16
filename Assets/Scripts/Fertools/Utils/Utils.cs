using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// This Works best with orthographic cameras
    /// </summary>
    /// <param name="cam"></param>
    /// <returns></returns>
    public static Vector3 GetMouseWorldPosition(Camera cam)
    {
        Vector3 position = cam.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        return position;
    }
    
    
    //TODO: Create MouseToWorldPosition that'll work with perspective cameras

    public static bool LayerMaskCompare(LayerMask mask, GameObject objectToCompare)
    {
        return (mask.value & 1 << objectToCompare.gameObject.layer) != 0;
    }
    
}
