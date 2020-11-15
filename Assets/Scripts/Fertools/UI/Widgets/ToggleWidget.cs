using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWidget : ItemSpinnerWidget
{
   protected override void Start()
   {
      CurrentValue = options[startIndex];
      currentIndex = startIndex;

      leftButton.onClick.AddListener(() =>
      {
         currentIndex++;
         currentIndex %= options.Length;
         CurrentValue = options[currentIndex];

         
      });
   }
}
