using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static string LoadingScreenSceneName = "Loading Scene";

    public CanvasGroup loadingProgressBar;
    public TextMeshProUGUI loadingText;
    
    public float startFadeDuration = .2f;
    public float progressBarSpeed = .2f;
    public float exitFadeDuration = .2f;
    public float loadCompleteDelay = .2f;

    private AsyncOperation _asyncOperation;
    private static string _sceneToLoad = "";
    private float _fillTarget = 0f;
    private string _loadingTextValue;

    public static void LoadScene(string sceneToLoad)
    {
        _sceneToLoad = sceneToLoad;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (LoadingScreenSceneName != null)
        {
            SceneManager.LoadScene(LoadingScreenSceneName);
        }
    }

    private void Start()
    {
        _loadingTextValue = loadingText.text;
        if (_sceneToLoad != "")
        {
            StartCoroutine(LoadAsynchronously());
        }
    }

    private void Update()
    {
        Time.timeScale = 1f;
        loadingProgressBar.GetComponent<Image>().DOFillAmount(_fillTarget, Time.deltaTime * progressBarSpeed);
    }
    
    private IEnumerator LoadAsynchronously()
    {
        LoadingInit();
        
        //begin scene loading
        _asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
        _asyncOperation.allowSceneActivation = false;

        //while the scene is loading we assign its progress to a target that we will use to fill the progress bar 
        while (_asyncOperation.progress < .9f)
        {
            _fillTarget = _asyncOperation.progress;
            yield return null;
        }
        //when the load is close to the end (usually it would never reach 1f) we set the bar to 100%
        _fillTarget = 1f;

        while (loadingProgressBar.GetComponent<Image>().fillAmount != _fillTarget)
        {
            yield return null;
        }
        
        //at this point the loading is now complete and we can do other stuff
        LoadingComplete();

        yield return new WaitForSeconds(loadCompleteDelay);
        
        //Fade to black
        //FadeInEvent fadeIn = new FadeInEvent(exitFadeDuration);
        //fadeIn.FireEvent();
        
        yield return new WaitForSeconds(exitFadeDuration);
        
        _asyncOperation.allowSceneActivation = true;
    }

    private void LoadingInit()
    {
        //DO Fade
        //FadeOutEvent fadeOut = new FadeOutEvent(startFadeDuration);
        //fadeOut.FireEvent();
        loadingProgressBar.GetComponent<Image>().fillAmount = 0f;
        loadingText.text = _loadingTextValue;
    }

    private void LoadingComplete()
    {
        loadingProgressBar.DOFade(0, .1f);
    }
}
