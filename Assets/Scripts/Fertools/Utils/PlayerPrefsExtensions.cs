using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefsExtensions
{

    public static void SetBool(this PlayerPrefs prefs, string name, bool booleanValue) 
    {
        PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
    }
 
    public static bool GetBool(this PlayerPrefs prefs, string name)  
    {
        return PlayerPrefs.GetInt(name) == 1;
    }
 
    /*
    public static bool GetBool(this PlayerPrefs prefs, string name, bool defaultValue)
    {
        if(PlayerPrefs.HasKey(name)) 
        {
            return GetBool(name);
        }
 
        return defaultValue;
    }
    */
    
}
