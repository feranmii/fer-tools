using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Objectives/Target Objective")]
public class TargetObjective : MissionObjective
{
    public int target;
    public int current;


    public override void Init()
    {
        base.Init();

        current = 0;
    }

    public override void Update()
    {
        base.Update();
        if (objectiveStatus == ObjectiveStatus.COMPLETED || objectiveStatus == ObjectiveStatus.FAILED)
            return;


        Debug.Log("Target");

        if (current >= target)
        {
            SetObjectiveStatus(ObjectiveStatus.COMPLETED);
        }
    }

    public void IncreaseCurrentValue()
    {
        current++;
    }
}