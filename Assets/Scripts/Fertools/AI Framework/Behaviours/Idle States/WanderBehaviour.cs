using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderBehaviour : BaseState
{
    private bool hasTarget;

    public override void Enter()
    {
    }

    public override void Execute()
    {
        if (!hasTarget)
        {
            if (!aiAgent.keyTransform)
            {
                targetVector = GetRandomPosition(aiAgent.transform.position);
            }
            else
            {
                targetVector = GetRandomPosition(aiAgent.keyTransform.position);
            }

            hasTarget = true;
        }
        else if (!navInterface.PathPending() &&
                 navInterface.GetRemainingDistance() < aiAgent.agentStats.distanceToChooseNewWanderPoint)
        {
            hasTarget = false;
        }

        Vector3 GetRandomPosition(Vector3 origin)
        {
            var retVal = Vector3.zero;


            NavMeshHit hit;

            var wanderRadius = aiAgent.agentStats.wanderRadius;
            var point = (origin + Random.insideUnitSphere * Random.Range(wanderRadius.x, wanderRadius.y));

            point.y = .3f;

            NavMesh.SamplePosition(point, out hit, 1f, NavMesh.AllAreas);


            retVal = hit.position;


            return retVal;
        }
    }

    public override void Exit()
    {
    }

    public WanderBehaviour(AIAgent aiAgent) : base(aiAgent)
    {
    }
}
