using System.Collections;
using System.Collections.Generic;
using Fertools.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
   
   public void Pause()
   {
      var tse = new TimeScaleEvent(0, 0 , false, 0, true);
      tse.FireEvent();
      
   }

   public void Resume()
   {
      var tse = new TimeScaleEvent(1, 0 , false, 0, true);
      tse.FireEvent();

   }
   
   public void Restart()
   {
      var tse = new TimeScaleEvent(1, 0 , false, 0, true);
      tse.FireEvent();
      LoadingScene.LoadScene(SceneManager.GetActiveScene().name);
   }


   public void QuitToMenu()
   {
      var tse = new TimeScaleEvent(1, 0 , false, 0, true);
      tse.FireEvent();
      LoadingScene.LoadScene(SceneNames.MENU);
   }
   
}

