using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EventCallbacks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Fertools.Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class Fader : MonoBehaviour
    {
        //Public Fields
        [BoxGroup("Fading Properties")]
        public float fadedAlpha;
        [BoxGroup("Fading Properties")]
        public float fullAlpha;

        [BoxGroup("Time Properties")] public float defaultDuration = .2f;

        [BoxGroup("Interaction")] public bool shouldBlockInteraction = false;
        
        //Private Fields

        private CanvasGroup _canvasGroup;
        private Image _image;
        
        
        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = fadedAlpha;

            _image = GetComponent<Image>();
            _image.enabled = false;
        }

        private void OnEnable()
        {
            OnFade.RegisterListener(DoFadeEvent);
            OnFadeIn.RegisterListener(DoFadeInEvent);
            OnFadeOut.RegisterListener(DoFadeOutEvent);
        }

        private void OnDisable()
        {
            OnFade.UnregisterListener(DoFadeEvent);
            OnFadeIn.UnregisterListener(DoFadeInEvent);
            OnFadeOut.UnregisterListener(DoFadeOutEvent);
        }

        private void DoFadeEvent(OnFade fade)
        {
            var target = (_canvasGroup.alpha >= 1) ? fadedAlpha : fullAlpha;
            Fade(_canvasGroup.alpha, target, fade.duration);
        }
        
        private void DoFadeInEvent(OnFadeIn fade)
        {
            
            Fade(_canvasGroup.alpha, fullAlpha, fade.duration);
        }

        private void DoFadeOutEvent(OnFadeOut fade)
        {
            Fade(_canvasGroup.alpha, fadedAlpha, fade.duration);
        }

        
        protected virtual void Fade(float startingAlpha, float targetAlpha, float duration = 1)
        {
            _canvasGroup.alpha = startingAlpha;
            _image.enabled = true;
            
            if (shouldBlockInteraction)
            {
                _canvasGroup.blocksRaycasts = true;
            }
            
            _canvasGroup.DOFade(targetAlpha, duration).OnComplete(() =>
            {
                print("Done");
                StopFading();
            });
        }
        
     
        protected virtual void StopFading()
        {
            print("Stopped Fading");

            if (_canvasGroup.alpha <= fadedAlpha)
            {
                _image.enabled = false;
                
                if (shouldBlockInteraction)
                {
                    _canvasGroup.blocksRaycasts = false;
                }
            }

           
        }
        
        
        [Button]
        public void FadeEvent()
        {
            OnFade fade  = new OnFade(defaultDuration);
            fade.FireEvent();
        }
        
        [Button]
        public void FadeInOneSecond()
        {
            OnFadeIn fade  = new OnFadeIn(defaultDuration);
            fade.FireEvent();
        }
            
        [Button]
        public void FadeOutOneSecond()
        {
            OnFadeOut fade  = new OnFadeOut(defaultDuration);
            fade.FireEvent();
        }
    }
}

public class OnFade : Event<OnFade>
{
    public float duration;



    public OnFade(float duration)
    {
        this.duration = duration;
    }
}
public class OnFadeIn : Event<OnFadeIn>
{
    public float duration;
    
    public OnFadeIn(float duration)
    {
        this.duration = duration;
    }
}

public class OnFadeOut : Event<OnFadeOut>
{
    public float duration;
    
    public OnFadeOut(float duration)
    {
        this.duration = duration;
    }
}

