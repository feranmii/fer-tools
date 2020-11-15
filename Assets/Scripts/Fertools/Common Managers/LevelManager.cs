using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using NaughtyAttributes;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Button()]
    public void WinLevel()
    {
        var lwe = new LevelWinEvent();
        lwe.FireEvent();
        var tse = new TimeScaleEvent(0, 3, false, 1f, true);
        tse.FireEvent();
    }

    [Button()]
    public void LoseLevel()
    {
        var lle = new LevelLoseEvent();
        lle.FireEvent();
        var tse = new TimeScaleEvent(0, 3, false, 1f, true);
        tse.FireEvent();
    }
}

public class LevelWinEvent : Event<LevelWinEvent>
{
}

public class LevelLoseEvent : Event<LevelLoseEvent>
{
}