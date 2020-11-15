using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Utils;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
   public Button playButton;
   public Button storeButton;
   public Button settingsButton;
   public Button quitButton;

   
   private UIScreenManager _screenManager;

   private void Awake()
   {
       _screenManager = FindObjectOfType<UIScreenManager>();
      
   }

   private void Start()
   {
       if(_screenManager == null)
           Debug.LogError("Screen manager not in scene");
       
       playButton.onClick.AddListener(() =>
       {
           LoadingScene.LoadScene(SceneNames.GAME);
       });
     
       InitBackButtons();
   }

   private void InitBackButtons()
   {
     
   }
   
   
}
