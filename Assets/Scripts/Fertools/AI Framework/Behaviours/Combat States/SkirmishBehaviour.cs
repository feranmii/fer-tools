using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;


public class SkirmishBehaviour : BaseState
{
    public float minDistFromTarget = 7f;
    public float maxDistFromTarget = 20f;
    bool haveADestTarget = false;
    int framesUntilCanReachObject = 0;
    public bool canCrossBehindTarget = true;
    public float maxTimeToWaitAtEachPoint = 3f;
    float timeLeftAtThisPoint;


    public SkirmishBehaviour(AIAgent aiAgent) : base(aiAgent)
    {
        minDistFromTarget = aiAgent.agentStats.minSkirmishDistFromTarget;
        maxDistFromTarget = aiAgent.agentStats.maxSkirmishDistFromTarget;
        canCrossBehindTarget = aiAgent.agentStats.canCrossBehindTarget;
        maxTimeToWaitAtEachPoint = aiAgent.agentStats.maxTimeToWaitAtEachSkirmishPoint;
    }

    public override void Tick()
    {
        framesUntilCanReachObject--;
        timeLeftAtThisPoint -= AIController.Instance.agentCycleTime;
    }

    public override void Execute()
    {
        if (haveADestTarget)
        {
            Debug.DrawLine(transform.position, targetVector, Color.yellow, 0.2f);
        }

        if (!haveADestTarget && timeLeftAtThisPoint <= 0)
        {
            targetVector = GetNewDestTarget(aiAgent.targetTransform);
        }

        else if (haveADestTarget && framesUntilCanReachObject < 0 && navInterface.ReachedDestination())
        {
            haveADestTarget = false;
            //wait
            timeLeftAtThisPoint = maxTimeToWaitAtEachPoint * Random.value;
        }
    }

    private Vector3 GetNewDestTarget(Transform targ)
    {
        Vector3 directionVector = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f);

        //Make the agent choose a position between the agent and the target, if desired.  This will get us a new "circle strafing" effect.
        if (!canCrossBehindTarget && Vector3.Dot(directionVector, targ.position - transform.position) > 0)
        {
            directionVector *= -1;
        }

        directionVector = directionVector.normalized;

        //Get a spot within the combat range
        Vector3 returnPos = targ.position + (directionVector * (Random.Range(minDistFromTarget, maxDistFromTarget)));

        if (!returnPos.Equals(Vector3.zero))
        {
            RaycastHit hit;
            //Get closer than the default range if we have to in order to fit, otherwise our agent will not pursue into close quarters
            if (Physics.Linecast(targ.position, returnPos + new Vector3(0, aiAgent.agentStats.dodgingClearHeight, 0),
                out hit, layerMask.value))
            {
                framesUntilCanReachObject = 5;
                haveADestTarget = true;
                return hit.point;
            }

            framesUntilCanReachObject = 5;
            haveADestTarget = true;
            return returnPos;
        }

        return targ.position;
    }

    public override void Exit()
    {
        haveADestTarget = false;
    }
}