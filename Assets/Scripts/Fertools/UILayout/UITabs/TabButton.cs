using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    [Space]
    public TabColors buttonColors;
    public TabColors textColors;

    
    [Space]
    [Header("References")]
    public Image background;
    public TextMeshProUGUI buttonText;
    
   
    [Space]
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    
    
    private void Awake()
    {
        background = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        tabGroup.Subscribe(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        onTabSelected.Invoke();
    }

    public void Deselect()
    {
        onTabDeselected.Invoke();
    }
}
