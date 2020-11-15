using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventCallbacks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScreenManager : MonoBehaviour
{
    [Foldout("Debug"), ReadOnly] public GameObject currentlyOpen;
    [ReorderableList] public List<GameObject> allScreens;
    public bool setFirstScreen;
    [Header("Screens Info"), ShowIf("setFirstScreen")] [Space] public GameObject firstScreen;

    private Stack<GameObject> openedScreens;

    private void Start()
    {
        if (!setFirstScreen && allScreens.Count > 0)
        {
            firstScreen = allScreens[0];
        }
      
        Init();
    }

    private void Init()
    {
        openedScreens = new Stack<GameObject>();

        if (firstScreen != null)
        {
            OpenScreen(firstScreen);
        }
        else
        {
            Debug.LogError("First Screen has not been Assigned!");
        }
    }


    public void OpenScreen(GameObject target)
    {
        var screen = allScreens.Find(o => o == target);
        if (screen != null)
        {
            if (!openedScreens.Contains(screen))
            {
                openedScreens.Push(screen);
            }

            if (currentlyOpen != null)
                currentlyOpen.gameObject.SetActive(false); //currentlyOpen.GetComponent<UIView>().Hide();

            // screen.GetComponent<UIView>().Show();
            screen.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Could not find the target screen");
        }

        currentlyOpen = openedScreens.Peek();

        StartCoroutine(SelectFirstSelectable());
    }

   
    public void CloseCurrentScreen()
    {
        openedScreens.Pop();
        //currentlyOpen.GetComponent<UIView>().Hide();
        currentlyOpen.gameObject.SetActive(false);


        var screenToOpen = openedScreens.Peek();

        OpenScreen(screenToOpen);
    }
    
    public void OpenNextScreen()
    {
        var currentIndex = allScreens.FindIndex(o => o == currentlyOpen);

        var nextScreenIndex = currentIndex + 1;

        OpenScreen(allScreens[nextScreenIndex]);
    }
    
    private IEnumerator SelectFirstSelectable()
    {
        var selectables = currentlyOpen.GetComponentsInChildren<Selectable>();

        if (selectables.Length > 0)
        {
            var selectableItem =
                (from selectable in selectables
                    where selectable.IsActive() && selectable.IsInteractable()
                    select selectable.gameObject).FirstOrDefault();

            EventSystem.current.SetSelectedGameObject(null);
            yield return null;

            EventSystem.current.SetSelectedGameObject(selectableItem);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }


}