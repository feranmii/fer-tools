using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [Header("Tab Buttons"), ReorderableList]
    public List<TabButton> tabButtons;

    [Space]
    
    //[ReorderableList]
   // public List<GameObject> pages;

    [Space] public PanelGroup panelGroup;
    public TabButton selectedTab;

    private void Start()
    {
        OnTabSelected(tabButtons[0]);
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();

        if (selectedTab == null || button != selectedTab)
        {
            button.background.color = button.buttonColors.colorHover;
            button.buttonText.color = button.textColors.colorHover;

        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
        
    }

    public void OnTabSelected(TabButton tab)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        
        selectedTab = tab;
        selectedTab.Select();
        
        ResetTabs();
        tab.background.color = tab.buttonColors.colorActive;
        tab.buttonText.color = tab.textColors.colorActive;

        //int index = tab.transform.GetSiblingIndex();
        //
        // for (int i = 0; i < pages.Count; i++)
        // {
        //     pages[i].SetActive(i == index);
        // }
        
        if(panelGroup != null)
            panelGroup.SetPageIndex(tab.transform.GetSiblingIndex());
    }

    public void ResetTabs()
    {
        foreach (var button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) continue;
            
            button.background.color = button.buttonColors.colorIdle;
            button.buttonText.color = button.textColors.colorIdle;

        }
    }
}

[System.Serializable]
public struct TabColors
{ 
    public Color colorIdle;
    public Color colorHover;
    public Color colorActive;
}