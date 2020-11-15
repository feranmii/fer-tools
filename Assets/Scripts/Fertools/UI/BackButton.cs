using System;
using UnityEngine;
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
        button.onClick.AddListener(() =>
        {
            _screenManager.CloseCurrentScreen();
            
        });
    }
}