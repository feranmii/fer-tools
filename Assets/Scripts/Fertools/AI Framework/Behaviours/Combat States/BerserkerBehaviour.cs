using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerBehaviour : BaseState
{
    float minDistToTargetIfNotInCover = 0;
    
    
    
    public override void Execute()
    {
        //Run to the key transform if we have one
        if (aiAgent.keyTransform)
        {
            targetVector = aiAgent.keyTransform.position;
        }
        //Otherwise, run at the target we are firing on
        else if (aiAgent.targetTransform)
        {
            if (Vector3.SqrMagnitude(transform.position - aiAgent.targetTransform.position) >
                minDistToTargetIfNotInCover
                || Physics.Linecast(aiAgent.GetEyePos(), aiAgent.targetTransform.position, layerMask))
            {
                targetVector = aiAgent.targetTransform.position;
            }
            else
            {
                targetVector = transform.position;
            }
            
            //targetVector = baseScript.targetTransform.position;
            
        }
    }

    public BerserkerBehaviour(AIAgent aiAgent) : base(aiAgent)
    {
        minDistToTargetIfNotInCover = aiAgent.agentStats.minDistToTargetIfNotInCover *
                                      aiAgent.agentStats.minDistToTargetIfNotInCover;
    }
}