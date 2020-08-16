using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Objectives/Timed Objective")]
public class TimedObjective : MissionObjective
{
    public bool failOnTimeFinished;

    public float targetTime;

    [SerializeField] private float _t;

    [SerializeField] private bool isSuccess;

    public override void Init()
    {
        base.Init();
        _t = 0;
        isSuccess = false;
    }

    public override void Update()
    {
        base.Update();
        if (objectiveStatus == ObjectiveStatus.COMPLETED || objectiveStatus == ObjectiveStatus.FAILED)
            return;


        _t += Time.deltaTime;

        if (_t >= targetTime)
        {
            if (!failOnTimeFinished)
            {
                isSuccess = true;
            }
            else
            {
                SetObjectiveStatus(ObjectiveStatus.FAILED);
            }
        }

        if (isSuccess)
        {
            SetObjectiveStatus(ObjectiveStatus.COMPLETED);
        }

        Debug.Log("Timed");
    }

    public void SetSuccess()
    {
        isSuccess = true; 
    }
}