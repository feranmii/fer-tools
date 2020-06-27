using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PanelGroup : MonoBehaviour
{
   [ReorderableList]
   public GameObject[] panels;

   public TabGroup tabGroup;

   public int panelIndex;

   private void Awake()
   {
      ShowCurrentPanel();
   }

   private void ShowCurrentPanel()
   {
      for (int i = 0; i < panels.Length; i++)
      {
         panels[i].SetActive(i == panelIndex);
      }
   }

   public void SetPageIndex(int index)
   {
      panelIndex = index;
      ShowCurrentPanel();
   }
}
