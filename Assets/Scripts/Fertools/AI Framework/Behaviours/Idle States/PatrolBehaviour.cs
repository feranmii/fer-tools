using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PatrolBehaviour : BaseState
{
    private bool havePatrolTarget = false;
    private int currentPatrolIndex = 0;
    private float patrolNodeDistSquared;


    private float t;

    public override void Enter()
    {
    }

    public override void Execute()
    {
        if (aiAgent.patrolNodes.Length >= 0)
        {
            if (!havePatrolTarget)
            {
                SetPatrolNodeDistSquared();

                targetVector = aiAgent.patrolNodes[currentPatrolIndex].position;

                havePatrolTarget = true;

                if (aiAgent.randomizePatrol)
                {
                    currentPatrolIndex = Random.Range(0, aiAgent.patrolNodes.Length);
                }
                else
                {
                    currentPatrolIndex++;

                    if (currentPatrolIndex >= aiAgent.patrolNodes.Length)
                    {
                        currentPatrolIndex = 0;
                    }
                }
            }
            else if (Vector3.SqrMagnitude(targetVector - aiAgent.mTransform.position) < patrolNodeDistSquared)
            {
                if (aiAgent.agentStats.waitAtPatrolNode)
                {
                    //Compensate for the AI Cycle time 
                    t += Time.deltaTime * AIController.Instance.agentCycleTime * 100;
                    
                    if (t >= aiAgent.agentStats.waitTimeAtPatrolNode)
                    {
                        t = 0;

                        havePatrolTarget = false;
                    }
                }
                else
                {
                    havePatrolTarget = false;
                }
            }
        }
        else
        {
            Debug.LogError("You have not assigned the patrol nodes");
        }

        void SetPatrolNodeDistSquared()
        {
            patrolNodeDistSquared = aiAgent.agentStats.stopDistanceToPatrolPoint *
                                    aiAgent.agentStats.stopDistanceToPatrolPoint;
        }
    }


    public override void Exit()
    {
    }

    public PatrolBehaviour(AIAgent aiAgent) : base(aiAgent)
    {
        
    }
}



