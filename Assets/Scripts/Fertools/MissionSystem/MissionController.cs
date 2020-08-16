using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MissionController : Singleton<MissionController>
{
    [ReorderableList] public List<MissionObjective> missionObjectivesList;

    [ReadOnly] public MissionObjective currentObjective;
    // [ReadOnly] public int currentObjectiveIndex;


    public Action onMissionComplete;
    public Action onMissionFailed;
    public Action onMissionStarted;


    
    protected override void Awake()
    {
        //Init
        missionObjectivesList.ForEach(objective =>
        {
            objective.Init();
            objective.onObjectiveComplete += OnObjectiveComplete;
            objective.onObjectiveFailed += OnObjectiveFailed;
            objective.onObjectiveStatusChanged += OnObjectiveStatusChanged;
        });
        
        currentObjective = missionObjectivesList[0];
        currentObjective.SetObjectiveStatus(MissionObjective.ObjectiveStatus.INPROGRESS);

        
    }

    private void Update()
    {
        //For Pararrel objectives
        /*for (int i = 0; i < missionObjectivesList.Count; i++)
        {
            var objective = missionObjectivesList[i];

            if (objective.objectiveStatus == MissionObjective.ObjectiveStatus.INPROGRESS)
            {
                objective.Update();
            }
        }*/
       
        if (currentObjective != null)
        {
            currentObjective.Update();
        }
    }


    public void AddObjective(MissionObjective objective)
    {
        if (!missionObjectivesList.Contains(objective))
            missionObjectivesList.Add(objective);
    }


    private void OnObjectiveFailed(MissionObjective objective)
    {
        if (objective.objectiveType == MissionObjective.ObjectiveType.PRIMARY)
        {
            onMissionFailed?.Invoke();
        }
        else
        {
            //Else just go to the next object
            GoToNextObjective();
        }
    }

    private void OnObjectiveStatusChanged(MissionObjective objective)
    {
    }

    private void OnObjectiveComplete(MissionObjective objective)
    {
       GoToNextObjective();
    }

    void GoToNextObjective()
    {
        var index = missionObjectivesList.FindIndex(missionObjective => missionObjective == currentObjective);

        if (index + 1 < missionObjectivesList.Count)
        {
            index++;

            currentObjective = missionObjectivesList[index];
            currentObjective.SetObjectiveStatus(MissionObjective.ObjectiveStatus.INPROGRESS);
        }
        else
        {
            onMissionComplete?.Invoke();
        }
    }
}