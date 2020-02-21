using System.Collections;
using System.Collections.Generic;
using Fertools.Utils;
using UnityEngine;

public class LocalStorage : MonoBehaviour
{
    public static int PlayerLevel
    {
        get => PlayerPrefs.GetInt(StaticStrings.USERLEVELID);
        set => PlayerPrefs.SetInt(StaticStrings.USERLEVELID, value);
    }
    
    public static int Xp 
    {
        get => PlayerPrefs.GetInt(StaticStrings.USERLEVELID);
        set => PlayerPrefs.SetInt(StaticStrings.USERLEVELID, value);
    }   
    public static int Cash 
    {
        get => PlayerPrefs.GetInt(StaticStrings.USERCASH);
        set => PlayerPrefs.SetInt(StaticStrings.USERCASH, value);
    }
    public static int Highscore 
    {
        get => PlayerPrefs.GetInt(StaticStrings.HIGHSCORE);
        set => PlayerPrefs.SetInt(StaticStrings.HIGHSCORE, value);
    }
    
    public static string UserName
    {
        get => PlayerPrefs.GetString(StaticStrings.USERNAME);
        set => PlayerPrefs.GetString(StaticStrings.USERNAME, value);
    }
    
}
