using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class TacticalBehaviour : BaseState
{
    float maxTimeInCover = 10f;
    float minTimeInCover = 5f;
    bool foundDynamicCover = false;
    float minDistToTargetIfNotInCover = 5f;
    float maxTimeTilNoCoverCharge = 3;
    float timeTilNoCoverCharge = 0;

    public override void Enter()
    {
        maxTimeInCover = aiAgent.agentStats.maxTimeInCover;
        minTimeInCover = aiAgent.agentStats.minTimeInCover;
        minDistToTargetIfNotInCover = aiAgent.agentStats.minDistToTargetIfNotInCover *
                                      aiAgent.agentStats.minDistToTargetIfNotInCover;
        timeTilNoCoverCharge = maxTimeTilNoCoverCharge;
    }

    public override void Tick()
    {
        if (!(aiAgent.currentCoverNode || foundDynamicCover))
        {
            timeTilNoCoverCharge -= Time.deltaTime;
        }
    }

    public override void Execute()
    {
        if (aiCoverFinder)
        {
            //Choose which part of the cover node we should move to based on whether we are suppressed and firing.

            if (aiAgent.useAdvancedCover || (!aiShooting.IsFiring() || !aiAgent.shouldFireFromCover))
            {

                targetVector = aiAgent.currentCoverNodePos;
            }
            else
            {
                
                targetVector = aiAgent.currentCoverNodeFiringPos;
            }

            if (aiAgent.currentCoverNode || foundDynamicCover)
            {

                if (navInterface.PathPartial())
                {

                    LeaveCover();
                }

                if (!aiAgent.inCover && navInterface.ReachedDestination())
                {

                    aiAgent.inCover = true;
                    SetTimeToLeaveCover(Random.Range(minTimeInCover, maxTimeInCover));
                }
            }
            else
            {
                var coverData = aiCoverFinder.FindCover(aiAgent.targetTransform, aiAgent.keyTransform);

                if (coverData.foundCover)
                {

                    SetCover(coverData.hidingPosition, coverData.firingPosition, coverData.isDynamicCover,
                        coverData.coverNode);
                    //Play vocalization
                    // if (soundScript)
                    //     soundScript.PlayCoverAudio();
                }
                //If we can't find cover, charge at our target.
                else if (aiAgent.targetTransform && timeTilNoCoverCharge < 0)
                {
                    NoCoverFoundDest();
                }
            }
        }
        else if (aiAgent.targetTransform && timeTilNoCoverCharge < 0)
        {
            NoCoverFoundDest();
        }
    }


    void NoCoverFoundDest()
    {
        if (Vector3.SqrMagnitude(transform.position - aiAgent.targetTransform.position) > minDistToTargetIfNotInCover
            || Physics.Linecast(aiAgent.GetEyePos(), aiAgent.targetTransform.position, layerMask))
        {
            targetVector = aiAgent.targetTransform.position;
        }
        else
        {
            targetVector = transform.position;
        }
    }

    async void SetTimeToLeaveCover(float timeToLeave)
    {
        while (timeToLeave > 0 && (aiAgent.currentCoverNode || foundDynamicCover))
        {
            if (aiAgent.inCover)
                timeToLeave--;
            else
                timeToLeave -= 0.25f;


            if (aiAgent.targetTransform)
            {
                //Makes the agent leave cover if it is no longer safe.  Uses the cover node's built in methods to check.
                if (!foundDynamicCover && !aiAgent.currentCoverNode.CheckForSafety(aiAgent.targetTransform.position))
                {
                    LeaveCover();
                }
                //Makes the agent leave cover if it is no longer safe. 
                else if (foundDynamicCover && !Physics.Linecast(aiAgent.currentCoverNodePos,
                             aiAgent.targetTransform.position, layerMask.value))
                {
                    LeaveCover();
                }
            }


            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        if (aiAgent.currentCoverNode || foundDynamicCover)
        {
            LeaveCover();
        }
    }


    //Called when the agent wants to leave cover.  Sets variables to values appropriate for an agent that is not in cover.
    public void LeaveCover()
    {
        if (aiAgent.currentCoverNode)
        {
            aiAgent.currentCoverNode.setOccupied(false);
            aiAgent.currentCoverNode = null;
        }
        else if (foundDynamicCover)
        {
            AIController.Instance.RemoveACoverSpot(aiAgent.currentCoverNodeFiringPos);
        }

        aiAgent.inCover = false;
        aiAgent.SetOrigStoppingDistance();

        foundDynamicCover = false;

        if (!aiAgent.shouldFireFromCover)
        {
            aiCoverFinder.ResetLastCoverPos();
        }

        if (aiAgent.useAdvancedCover)
        {
            //TODO: Use this
            //animationScript.EndAdvancedCover();
        }
    }


    //Used to set variables once cover is found.
    void SetCover(Vector3 newCoverPos, Vector3 newCoverFiringSpot, bool isDynamicCover, CoverNode newCoverNodeScript)
    {
        timeTilNoCoverCharge = maxTimeTilNoCoverCharge;

        aiAgent.currentCoverNodePos = newCoverPos;
        aiAgent.currentCoverNodeFiringPos = newCoverFiringSpot;

        navInterface.SetStoppingDistance(0);

        if (isDynamicCover)
        {
            foundDynamicCover = true;
            AIController.Instance.AddACoverSpot(aiAgent.currentCoverNodeFiringPos);
        }
        else
        {
            aiAgent.currentCoverNode = newCoverNodeScript;
            aiAgent.currentCoverNode.setOccupied(true);
            if (aiAgent.useAdvancedCover)
            {
                // aiAnimation.StartAdvancedCover(aiAgent.currentCoverNode.advancedCoverDirection, aiAgent.currentCoverNode.faceDir);
            }
        }
    }

    public override void Exit()
    {
        LeaveCover();
    }

    public TacticalBehaviour(AIAgent aiAgent) : base(aiAgent)
    {
    }
}