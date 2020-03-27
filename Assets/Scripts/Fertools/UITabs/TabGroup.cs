using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [Header("Tab Buttons"), ReorderableList]
    public List<TabButton> tabButtons;

    [Space]
    
    [ReorderableList]
    public List<GameObject> pages;
    
    [Space]
    
    [Header("Tab States")]
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    [Space]
    public TabButton selectedTab;
    
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
            button.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
        
    }

    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        
        selectedTab = button;
        selectedTab.Select();
        
        ResetTabs();
        button.background.sprite = tabActive;

        int index = button.transform.GetSiblingIndex();

        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == index);
        }
        
    }

    public void ResetTabs()
    {
        foreach (var button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) continue;
            
            button.background.sprite = tabIdle;
        }
    }
}
