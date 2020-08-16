using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class DebugDisableHUD : MonoBehaviour
{
    public bool disableOnStart;

    [Tag] public string hudTag;

    private GameObject[] allHUDs;

    void Start()
    {
        allHUDs = GameObject.FindGameObjectsWithTag(hudTag);
        if (disableOnStart)
            DisableHUD();
    }

    [Button()]
    void EnableHUD()
    {
        if (allHUDs.Length < 1)
        {
            Debug.LogError("Could not find any HUD with such Tag");
            return;
        }

        foreach (var o in allHUDs)
        {
            o.SetActive(true);
        }
    }

    [Button()]
    void DisableHUD()
    {
        if (allHUDs.Length < 1)
        {
            Debug.LogError("Could not find any HUD with such Tag");
            return;
        }


        foreach (var o in allHUDs)
        {
            o.SetActive(false);
        }
    }
}