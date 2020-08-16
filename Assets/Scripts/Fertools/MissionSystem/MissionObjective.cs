using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NaughtyAttributes;
using UnityEngine;


public class MissionObjective : ScriptableObject
{
    //TODO: Use FStateMachine for this
    public enum ObjectiveStatus
    {
        IDLE,
        FAILED,
        COMPLETED,
        INPROGRESS
    }
    
    //TODO: Use FStateMachine for this

    public enum ObjectiveType
    {
        PRIMARY,
        SECONDARY
    }

    [Foldout("Objective Info")] public ObjectiveType objectiveType;
    [Foldout("Objective Info"), ReadOnly] public ObjectiveStatus objectiveStatus = ObjectiveStatus.IDLE;

    [Space] [Foldout("Objective Info"), Header("Description")]
    public string objectiveName;

    [Foldout("Objective Info")] public string objectiveDescription;

    [Space] [Foldout("Reward")] public int missionRewardValue = 1;

    public Action<MissionObjective> onObjectiveComplete;
    public Action<MissionObjective> onObjectiveFailed;
    public Action<MissionObjective> onObjectiveStatusChanged;


    public virtual void Init()
    {
        SetObjectiveStatus(ObjectiveStatus.IDLE);
    }

    public virtual void Update()
    {
    }

    public void SetObjectiveStatus(ObjectiveStatus status)
    {
        var oldValue = objectiveStatus;
        var newValue = status;

        if (!newValue.Equals(oldValue))
        {
            objectiveStatus = status;

            onObjectiveStatusChanged?.Invoke(this);
        }


        switch (status)
        {
            case ObjectiveStatus.FAILED:
                onObjectiveFailed?.Invoke(this);
                break;
            case ObjectiveStatus.COMPLETED:
                onObjectiveComplete?.Invoke(this);
                break;
        }
    }
}