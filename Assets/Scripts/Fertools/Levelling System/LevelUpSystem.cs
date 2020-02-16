using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    public const float LEVEL_INCREASE_CONSTANT = 0.04f;
    
    public int UserLevel
    {
        get => LocalStorage.PlayerLevel;
        set => LocalStorage.PlayerLevel = value;
    }
    public int Xp
    {
        get => LocalStorage.Xp;
        set => LocalStorage.Xp = value;
    }

    public int XpNeeded;

    private void Start()
    {
        if (UserLevel < 1)
            UserLevel = 1;
        
        XpNeeded = Mathf.FloorToInt(Mathf.Pow(UserLevel, 2) / LEVEL_INCREASE_CONSTANT);
    }

    private void OnEnable()
    {
        OnXPGained.RegisterListener(ProcessXpGain);
    }    
    private void OnDisable()
    {
        OnXPGained.UnregisterListener(ProcessXpGain);
    }

    private void ProcessXpGain(OnXPGained gained)
    {
        Xp += gained.xpGained;

        
        
        print(Mathf.FloorToInt(LEVEL_INCREASE_CONSTANT * Mathf.Sqrt(Xp)));
        
        //UserLevel = Mathf.FloorToInt(LEVEL_INCREASE_CONSTANT * Mathf.Sqrt(Xp));
        UserLevel =  Mathf.FloorToInt(LEVEL_INCREASE_CONSTANT * Mathf.Sqrt(Xp));
        
    }
    
    

    private void Update()
    {
        
    }
}

public class OnXPGained : Event<OnXPGained>
{
    public int xpGained;

    public OnXPGained(int xpGained)
    {
        this.xpGained = xpGained;
    }
}


public class OnLevelUpgrade : Event<OnLevelUpgrade>
{
    public int newLevel;

    public OnLevelUpgrade(int newLevel)
    {
        this.newLevel = newLevel;
    }
}
