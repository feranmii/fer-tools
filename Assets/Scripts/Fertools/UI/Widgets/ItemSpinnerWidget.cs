using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class ItemSpinnerWidget : BaseSpinnerWidget<string>
{
    public string CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = value;

            UpdateText();
        }
    }


    [ReorderableList] public string[] options;
    public int startIndex = 0;
    [ReadOnly] public int currentIndex = 0;

    protected virtual void Start()
    {
        CurrentValue = options[startIndex];
        currentIndex = startIndex;
        
        rightButton.onClick.AddListener(() =>
        {
            currentIndex++;

            currentIndex = Mathf.Min(currentIndex, options.Length - 1);

            CurrentValue = options[currentIndex];
        });

        leftButton.onClick.AddListener(() =>
        {
            currentIndex--;

            currentIndex = Mathf.Max(0, currentIndex);

            CurrentValue = options[currentIndex];
        });
    }

    private void UpdateText()
    {
        valueText.text = CurrentValue;
    }

    private void OnValidate()
    {
        if (options.Length < 1)
        {
            startIndex = 0;
            return;
        }

        startIndex = Mathf.Clamp(startIndex, 0, options.Length - 1);
    }
}