using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using Fertools.Achievement;
using NaughtyAttributes;
using UnityEngine;


namespace Fertools.Achievement
{
    [System.Serializable]
    public class Achievement
    {
    public AchievementType achievementType;

    [BoxGroup("Info")]
    public string id;
    [BoxGroup("Info")]

    public string title;
    [BoxGroup("Info")]
    public bool unlockStatus;

    [HorizontalLine]
    [ShowIf("IsProgressiveAchievement")]
    public int currentProgress;

    [Space]
    [ShowIf("IsProgressiveAchievement")]
    public int progressTarget;
    
    [InfoBox("|Rewards could be XP or Points")]
    [Space]

    public int reward;
    
    
    
    public bool IsProgressiveAchievement()
    {
        return achievementType == AchievementType.Progressive;
    }

    public void UnlockAchievement()
    {
        unlockStatus = true;
        OnAchievementUnlocked achievementUnlocked = new OnAchievementUnlocked(this);
        achievementUnlocked.FireEvent();
    }

    public void LockAchievement()
    {
        unlockStatus = false;
    }

    public void AddProgress(int val)
    {
        if (achievementType == AchievementType.Simple)
        {
            UnlockAchievement();
        }

        if (achievementType == AchievementType.Progressive)
        {
            currentProgress += val;
            EvaluateProgress();
        }
        
    }

    public void SetProgress(int val)
    {
        currentProgress = val;
        EvaluateProgress();
    }

    public void EvaluateProgress()
    {
        if (currentProgress >= progressTarget)
        {
            currentProgress = progressTarget;
            UnlockAchievement();
        }
    }

    public void ResetAchievement()
    {
        unlockStatus = false;
        currentProgress = 0;
    }
}

    public enum AchievementType
    {
        Simple,
        Progressive
    }

}

public class OnAchievementUnlocked : Event<OnAchievementUnlocked>
{
    public Achievement Achievement;

    public OnAchievementUnlocked(Achievement achievement)
    {
        Achievement = achievement;
    }
}