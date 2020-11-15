using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour
{
    private UIScreenManager _screenManager;

    private void Awake()
    {
        _screenManager = FindObjectOfType<UIScreenManager>();
    }

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => { _screenManager.CloseCurrentScreen(); });

       
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                _screenManager.CloseCurrentScreen();
            }
        }
    }
}