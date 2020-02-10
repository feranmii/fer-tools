using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/String")]
public class StringVariable : ScriptableObject
{
    public bool isSerializable;
   
    public string value;


    private void OnEnable()
    {
        if(!isSerializable)
            value = "";
    }
    
}
