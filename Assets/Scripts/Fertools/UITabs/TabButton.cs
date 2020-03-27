using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;

    public Image background;
    
    [Space]
    public UnityEvent onTabSelected;
    public UnityEvent onTabDeselected;
    
    
    private void Start()
    {
        background = GetComponent<Image>();
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
