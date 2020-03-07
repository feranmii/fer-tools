using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using EventCallbacks;
using NaughtyAttributes;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenTransitionImageEffect : MonoBehaviour
{
    /// Provides a shader property that is set in the inspector
    /// and a material instantiated from the shader
    /// 
    public Shader shader;
    [Range(0,1.0f)]
    public float maskValue;
    public Color maskColor = Color.black;
    public Texture2D maskTexture;
    public bool maskInvert;

    public float duration = 1;
    private Material m_Material;
    private bool m_maskInvert;

    Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }
            return m_Material;
        }
    }

    private void OnEnable()
    {
        OnScreenTransition.RegisterListener(DoScreenTransition);
    }
    
    void OnDisable()
    {
        OnScreenTransition.UnregisterListener(DoScreenTransition);

        if (m_Material)
        {
            DestroyImmediate(m_Material);
        }
    }

    

    void Start()
    {
        maskValue = 0;
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        shader = Shader.Find("Hidden/ScreenTransitionImageEffect");

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (shader == null || !shader.isSupported)
            enabled = false;
    }

    [Button()]
    public void TestTransition()
    {
        OnScreenTransition screenTransition = new OnScreenTransition(maskValue, 0, duration );
        screenTransition.FireEvent();
    }

    private void DoScreenTransition(OnScreenTransition screenTransition)
    {
        var target = maskValue >= 1 ? 0 : 1;
        ScreenTransition(screenTransition.startingValue, target, screenTransition.duration);
    }

    protected virtual void ScreenTransition(float startingValue, float target, float duration = 1)
    {
        maskValue = startingValue;

        DOTween.To(() => maskValue, x => maskValue = x, target, duration);
    }

    

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!enabled)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetColor("_MaskColor", maskColor);
        material.SetFloat("_MaskValue", maskValue);
        material.SetTexture("_MainTex", source);
        material.SetTexture("_MaskTex", maskTexture);

        if (material.IsKeywordEnabled("INVERT_MASK") != maskInvert)
        {
            if (maskInvert)
                material.EnableKeyword("INVERT_MASK");
            else
                material.DisableKeyword("INVERT_MASK");
        }

        Graphics.Blit(source, destination, material);
    }
    
    
}

public class OnScreenTransition : Event<OnScreenTransition>

{
    public float startingValue;
    public float target;
    public float duration;

    public OnScreenTransition(float startingValue, float target, float duration)
    {
        this.startingValue = startingValue;
        this.target = target;
        this.duration = duration;
    }
}