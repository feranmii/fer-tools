using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Simple Class that assigns the name of the parent GameObject to a UI TextField
/// </summary>

[ExecuteInEditMode]
public class UIGetName : MonoBehaviour
{
    
    private void Update()
    {
        var textObject = GetComponent<TextMeshProUGUI>();

        if (textObject != null)
        {
            textObject.text = transform.parent.name;
            textObject.gameObject.name = $"{transform.parent.name} - Text";
        }
        
    }
}
