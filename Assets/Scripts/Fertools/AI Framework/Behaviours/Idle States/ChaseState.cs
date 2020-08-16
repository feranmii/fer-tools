using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    private float radiusToCallOfSearch;
    
    public ChaseState(AIAgent aiAgent) : base(aiAgent)
    {
        radiusToCallOfSearch = aiAgent.agentStats.radiusToCallOfSearch;
    }

    public override void Execute()
    {
        if (aiAgent.targetTransform && navInterface.GetRemainingDistance() < radiusToCallOfSearch)
        {
            targetVector = aiAgent.targetTransform.transform.position;
        }
        else if(!aiAgent.targetTransform)
        {
            targetVector = transform.position;
        }
    }
}
