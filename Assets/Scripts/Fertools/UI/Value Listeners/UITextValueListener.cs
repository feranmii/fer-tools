using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class UITextValueListener : MonoBehaviour
{
    public enum TextTargetType
    {
        INT,
        STRING,
        FLOAT,
    }
    
    public TextMeshProUGUI targetText;

    [Space] public TextTargetType textSourceType;

    [ShowIf("IsStringTarget")] public StringVariable stringVariable;
    [ShowIf("IsIntTarget")] public IntVariable intVariable;
    [ShowIf("IsFloatTarget")] public FloatVariable floatVariable;

    [Space] 
    public UnityEvent onValueChanged;
    
    //-----------Editor Checks------------

    bool IsStringTarget()
    {
        return textSourceType == TextTargetType.STRING;
    }

    bool IsFloatTarget()
    {
        return textSourceType == TextTargetType.FLOAT;
    }

    bool IsIntTarget()
    {
        return textSourceType == TextTargetType.INT;
    }

    //---------------------------------------


    private void OnEnable()
    {
        switch (textSourceType)
        {
            case TextTargetType.INT:
                intVariable.onValueChanged += OnValueChanged;
                targetText.text = intVariable.Value.ToString();
                break;
            case TextTargetType.STRING:
                stringVariable.onValueChanged += OnValueChanged;
                targetText.text = stringVariable.Value;

                break;
            case TextTargetType.FLOAT:
                floatVariable.onValueChanged += OnValueChanged;
                targetText.text = floatVariable.Value.ToString();

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnDisable()
    {
        switch (textSourceType)
        {
            case TextTargetType.INT:
                intVariable.onValueChanged -= OnValueChanged;
                break;
            case TextTargetType.STRING:
                stringVariable.onValueChanged -= OnValueChanged;
                break;
            case TextTargetType.FLOAT:
                floatVariable.onValueChanged -= OnValueChanged;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void OnValueChanged<T>(T value)
    {
        print(value.ToString());
        if (targetText != null)
        {
            targetText.text = value.ToString();
        }
        
        onValueChanged?.Invoke();
        
    }

    [Button("Check For Text")]
    public void CheckForText()
    {
        var tmp = GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            targetText = tmp;
        }
        else
        {
            Debug.LogError("Couldn't Find Text on this gameobject");
        }
    }
}