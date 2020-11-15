using System;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fertools.UI.Widgets
{
    public class NumberSpinnerWidget : BaseSpinnerWidget<int>
    {
        public int CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = value;

                if (currentValue < minValue)
                {
                    currentValue = minValue;
                }

                if (currentValue > maxValue)
                {
                    currentValue = maxValue;
                }

                UpdateText();
            }
        }

        [Min(0)] [Space] [Foldout("Range")] public int minValue = 0;
        [Space] [Foldout("Range")] public int step = 1;

        [Foldout("Range")] public int maxValue = 100;


        private void Start()
        {
            CurrentValue = minValue;
            rightButton.onClick.AddListener(() => { CurrentValue += step; });

            leftButton.onClick.AddListener(() => { CurrentValue -= step; });
        }

        private void UpdateText()
        {
            valueText.text = CurrentValue.ToString();
        }

        private void OnValidate()
        {
            if (maxValue < minValue)
            {
                maxValue = minValue;
            }

            //TODO: Fix this
            // if (minValue > maxValue)
            // {
            //     minValue = maxValue;
            // }
        }
    }
}