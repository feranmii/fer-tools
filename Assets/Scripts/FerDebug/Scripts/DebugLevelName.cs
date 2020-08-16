using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugLevelName : MonoBehaviour
{

    [Foldout("Level Name / Number"), ShowIf("showLevelName")] public RectTransform levelNameObject;
    [Foldout("Level Name / Number"), ShowIf("showLevelName")] public Canvas targetCanvas;
 
    private void Start()
    {
            InitLevelName();
     
    }

    private void InitLevelName()
    {
        var levelName = SceneManager.GetActiveScene().name;
        var levelNumber = SceneManager.GetActiveScene().buildIndex;

        var levelObject = Instantiate(levelNameObject, targetCanvas.transform);

        levelObject.anchorMin = new Vector2(1, 1);
        levelObject.anchorMax = new Vector2(1, 1);

        levelObject.anchoredPosition = new Vector2(-30, -70);
        levelObject.GetComponent<TextMeshProUGUI>().text = $"{levelName} ({levelNumber})";
        levelObject.transform.SetAsLastSibling();
    }
    
}