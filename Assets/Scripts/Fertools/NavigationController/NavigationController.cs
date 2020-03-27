using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class NavigationController 
{
    public static void LoadScene(int index)
    {
        //Reset Time Scale
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
        
        SceneManager.LoadScene(index);
    }

    public static void LoadScene(string sceneName)
    {
        
        //TODO: Reset Time in TimeManager

        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }
        
        SceneManager.LoadScene(sceneName);
    }

    public static void RestartScene()
    {
        
        if (Time.timeScale < 1)
        {
            Time.timeScale = 1;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void Quit()
    {
        Application.Quit();
    }
    

}

