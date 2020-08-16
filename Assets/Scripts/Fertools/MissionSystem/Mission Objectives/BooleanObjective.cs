using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Objectives/Boolean Objective")]
public class BooleanObjective : MissionObjective
{
    [SerializeField] private bool success;

    public override void Init()
    {
        base.Init();
        success = false;
    }

    public override void Update()
    {
        base.Update();

        if (objectiveStatus == ObjectiveStatus.COMPLETED || objectiveStatus == ObjectiveStatus.FAILED)
            return;
        
        if (success)
        {
            SetObjectiveStatus(ObjectiveStatus.COMPLETED);
        }

        Debug.Log("Location");
    }

    public void SetSucess()
    {
        success = true;
    }
    
    
}