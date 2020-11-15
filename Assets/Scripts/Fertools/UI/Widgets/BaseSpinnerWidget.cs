using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseSpinnerWidget<T> : MonoBehaviour
{
   [ReadOnly]
   public T currentValue;
   
   [Foldout("Buttons")] public Button leftButton;
   [Foldout("Buttons")] public Button rightButton;

   [Space] [Foldout("References")] public TextMeshProUGUI valueText;


}
