using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Menu<T> : Menu where T: Menu<T>
{
    public static T Instance { get; set; }
    
    
}

public abstract class Menu : MonoBehaviour
{
     
}
