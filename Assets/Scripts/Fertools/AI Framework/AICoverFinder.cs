using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class AICoverFinder : MonoBehaviour
{
    //Cover Seek Methods
    public CoverSeekMethods currentCoverSeekMethod = CoverSeekMethods.WithinCombatRange;

    public enum CoverSeekMethods
    {
        RandomCover = 0,
        WithinCombatRange = 1,
        AdvanceTowardsTarget = 2,
    }

    public enum DynamicCoverMethods
    {
        Raycasts = 1,
    }

    private Vector3 lastCoverPos;
    public float minDistBetweenLastCover = 20;
    private float minDistBetweenLastCoverSquared = 10;
    public float minCoverDistFromEnemy = 10;

    public float maxCoverDistFromEnemy = 50;

    //Use squared values for cheaper distance check via Vector3.SqrMagnitude
    private float maxCoverDistSqrd;
    private float minCoverDistSqrd;
    public float maxDistToCover = 9999;

    public float minDistToAdvance = 5;

    private CoverNode[] coverNodes;

    //Dynamic cover
    [Foldout("Dynamic Cover")] public DynamicCoverMethods dynamicCoverSelectionMode = DynamicCoverMethods.Raycasts;
    [Foldout("Dynamic Cover")] public bool shouldUseDynamicCover = true;

    [Foldout("Dynamic Cover"), ShowIf("shouldUseDynamicCover")]
    public bool useFirstDynamicCoverFound = true;

    [Foldout("Dynamic Cover"), ShowIf("shouldUseDynamicCover")]
    public float dynamicCoverMaxDistFromMe = 15f;

    [Foldout("Dynamic Cover")]

    //private var dynamicCoverMaxDistFromMeSqr : float;
    public float dynamicCoverNodeHeightOffset = 0.3f;

    [Foldout("Dynamic Cover"), ShowIf("shouldUseDynamicCover")]
    public float dynamicCoverNodeFireOffset = 1.5f;

    [Foldout("Dynamic Cover"), ShowIf("shouldUseDynamicCover")]
    public float dynamicCoverWidthNeededToHide = 1.5f;

    [Foldout("Dynamic Cover"), ShowIf("shouldUseDynamicCover")]
    public float maxDistBehindDynamicCover = 5;

    [Foldout("Dynamic Cover")] public bool useOnlyStaticCover = true;

    [Foldout("Dynamic Cover"), Header("Settings")]
    public int angleBetweenCasts = 10;

    [Foldout("Dynamic Cover")] public float maxRayDist = 1000f;
    [Foldout("Dynamic Cover")] public float rayCastHeightOffGround = 0.1f;
    [Foldout("Dynamic Cover")] public float distFromWallToBe = 1;


    public float defendingDist = 20;
    private float defendingDistSquared = 20;
    private Transform myTransform;

    [HideInInspector] public LayerMask layerMask;

    Vector3[] verts;

    NavmeshInterface navI;

    public int coverNodeGroup;


    private void Start()
    {
        navI = GetComponent<AIAgent>().navInterface;
        if (useOnlyStaticCover && shouldUseDynamicCover)
        {
            verts = navI.GetNavmeshVertices();
        }

        myTransform = transform;
        //dynamicCoverMaxDistFromMeSqr = dynamicCoverMaxDistFromMe*dynamicCoverMaxDistFromMe;			
        maxCoverDistSqrd = maxCoverDistFromEnemy * maxCoverDistFromEnemy;
        minCoverDistSqrd = minCoverDistFromEnemy * minCoverDistFromEnemy;
        layerMask = AIController.Instance.layerMask;
        defendingDistSquared = defendingDist * defendingDist;
        minDistToAdvance = minDistToAdvance * minDistToAdvance;

        if (AIController.Instance != null)
        {
            coverNodes = AIController.Instance.GetCovers();
        }
        else
        {
            Debug.LogError(
                "No Controller has been detected!  An AIController is required for the AI to work!  Please create a new gameObject and attach the Paragon AI ControllerScript to it!");
        }
    }

    public void ResetLastCoverPos()
    {
        lastCoverPos = new Vector3(100000, 100000, 100000);
    }

    public CoverData FindCover(Transform targetTransform)
    {
        return FindStaticCover(targetTransform, null);
    }

    public CoverData FindCover(Transform targetTransform, Transform transformToDefend)
    {
        return FindStaticCover(targetTransform, transformToDefend);
    }

    // Static Cover
    private CoverData FindStaticCover(Transform targetTransform, Transform transformToDefend)
    {
        var targetTransformPos = targetTransform.position;
        if (targetTransform && myTransform)
        {
            //Closest Covers
            if (currentCoverSeekMethod == CoverSeekMethods.WithinCombatRange)
            {
                return FindCoverWithinCombatRange(targetTransformPos, transformToDefend);
            }
            //Advance Towards Cover
             if (currentCoverSeekMethod == CoverSeekMethods.AdvanceTowardsTarget)
            {
                return FindAdvancingCover(targetTransformPos, transformToDefend);
            }
            //Random Cover
            if(currentCoverSeekMethod == CoverSeekMethods.RandomCover)
            {
                return FindRandomCover(targetTransformPos, transformToDefend);
            }
        }


        var bsData = new CoverData();
        return bsData;
    }

    //Agent will try and find cover that is within a given range of the target.
    CoverData FindCoverWithinCombatRange(Vector3 targetTransformPos, Transform transformToDefend)
    {
        var i = 0;
        var myPos = myTransform.position;
        CoverNode currentCoverNode = null;
        var closestDistSquared = maxDistToCover;

        //We will take cover outside of the desired range if we can't find any within.
        var foundCoverWithinAcceptableRange = false;
        for (i = 0; i < coverNodes.Length; i++)
        {

            //Check if the node we are checking is occupied and within acceptable distances to key points
            if (!coverNodes[i].isOccupied() && coverNodeGroup == coverNodes[i].coverNodeGroup &&
                Vector3.SqrMagnitude(coverNodes[i].GetPosition() - lastCoverPos) >
                minDistBetweenLastCoverSquared &&
                (!transformToDefend ||
                 Vector3.SqrMagnitude(coverNodes[i].GetPosition() - transformToDefend.position) <
                 defendingDistSquared))
            {
                print("final?");

                var distToTargetSquared = Vector3.SqrMagnitude(coverNodes[i].GetPosition() - targetTransformPos);
                var nodeCheckingNowDistSquared = Vector3.SqrMagnitude(myPos - coverNodes[i].GetPosition());
                //Check for line of sight
                if (coverNodes[i].ValidCoverCheck(targetTransformPos))
                {
                    print("Check complete?");

                    //Prefer nodes within othe agent's combat range
                    if (minCoverDistSqrd < distToTargetSquared && maxCoverDistSqrd > distToTargetSquared)
                    {
                        if (!foundCoverWithinAcceptableRange || (nodeCheckingNowDistSquared < closestDistSquared))
                        {
                            closestDistSquared = nodeCheckingNowDistSquared;
                            currentCoverNode = coverNodes[i];
                            foundCoverWithinAcceptableRange = true;
                        }
                    }
                    //Check if this is the closest so far 
                    else if (!foundCoverWithinAcceptableRange && nodeCheckingNowDistSquared < closestDistSquared)
                    {
                        closestDistSquared = nodeCheckingNowDistSquared;
                        currentCoverNode = coverNodes[i];
                    }
                }
            }
        }

        //pass the data to the script that asked for cover
        if (currentCoverNode != null)
        {

            lastCoverPos = currentCoverNode.GetPosition();
            return new CoverData(true, currentCoverNode.GetPosition(),
                currentCoverNode.GetSightNodePosition(), false, currentCoverNode);
        }

        //Only bother with dynamic cover if we need it
        if (shouldUseDynamicCover && !AIController.Instance.usePerformanceMode)
        {
            return FindDynamicCover(targetTransformPos, transformToDefend);
        }

        return new CoverData();
    }

    CoverData FindAdvancingCover(Vector3 targetTransformPos, Transform transformToDefend)
    {
        var i = 0;
        var myPos = myTransform.position;
        CoverNode currentCoverNodeScript = null;

        //Will find closest cover that is nearer than the last one we have if possible.
        //If not, we'll move to the target.
        Vector3 posToAdvanceTo;

        posToAdvanceTo = transformToDefend ? transformToDefend.position : targetTransformPos;

        var distBetweenMeAndTarget = Vector3.SqrMagnitude(myPos - posToAdvanceTo) - minDistToAdvance;
        var closestDistBetweenMeAndCover = distBetweenMeAndTarget;

        for (i = 0; i < coverNodes.Length; i++)
        {
            if (!coverNodes[i].isOccupied())
            {
                float sqrDistBetweenNodeAndTargetPos =
                    Vector3.SqrMagnitude(coverNodes[i].GetPosition() - posToAdvanceTo);
                //Check if we'll be closer to target than we stand now
                if (sqrDistBetweenNodeAndTargetPos < distBetweenMeAndTarget)
                {
                    //Check if this node is closest to us
                    if (Vector3.SqrMagnitude(coverNodes[i].GetPosition() - myPos) < closestDistBetweenMeAndCover)
                    {
                        //Check if node is safe
                        if (coverNodes[i].ValidCoverCheck(targetTransformPos))
                        {
                            closestDistBetweenMeAndCover = sqrDistBetweenNodeAndTargetPos;
                            currentCoverNodeScript = coverNodes[i];
                        }
                    }
                }
            }
        }

        if (currentCoverNodeScript != null)
        {
            lastCoverPos = currentCoverNodeScript.GetPosition();
            return new CoverData(true, currentCoverNodeScript.GetPosition(),
                currentCoverNodeScript.GetSightNodePosition(), false, currentCoverNodeScript);
        }

        //Dynamic advancing cover is NOT supported

        return new CoverData();
    }

    CoverData FindRandomCover(Vector3 targetTransformPos, Transform transformToDefend)
    {
        var i = 0;
        CoverNode currentCoverNodeScript = null;
        var availableCoverNodeScripts = new List<CoverNode>();

        //Fill a list with potential nodes
        for (i = 0; i < coverNodes.Length; i++)
        {
            if (!coverNodes[i].isOccupied())
            {
                if (coverNodes[i].ValidCoverCheck(targetTransformPos) &&
                    (!transformToDefend ||
                     Vector3.SqrMagnitude(coverNodes[i].GetPosition() - transformToDefend.position) <
                     defendingDistSquared))
                {
                    availableCoverNodeScripts.Add(coverNodes[i]);
                }
            }
        }

        if (availableCoverNodeScripts.Count > 0)
        {
            //Pick a random node
            currentCoverNodeScript = availableCoverNodeScripts[Random.Range(0, availableCoverNodeScripts.Count)];
            lastCoverPos = currentCoverNodeScript.GetPosition();

            return new CoverData(true, currentCoverNodeScript.GetPosition(),
                currentCoverNodeScript.GetSightNodePosition(), false, currentCoverNodeScript);
        }

        //Only bother with dynamic cover if we need it
        if (shouldUseDynamicCover && !AIController.Instance.usePerformanceMode)
        {
            return FindDynamicCover(targetTransformPos, transformToDefend);
        }

        return new CoverData();
    }

    CoverData FindDynamicCover(Vector3 targetTransformPos, Transform transformToDefend)
    {
        if (dynamicCoverSelectionMode == DynamicCoverMethods.Raycasts)
        {
            return FindRaycastDynamicCover(targetTransformPos, transformToDefend);
        }


        if (!useOnlyStaticCover)
        {
            verts = navI.GetNavmeshVertices();
        }

        Vector3 myPos;
        if (!transformToDefend)
            myPos = myTransform.position;
        else
            myPos = transformToDefend.position;
        myPos.y += dynamicCoverNodeHeightOffset;
        var hideOffset = dynamicCoverNodeFireOffset - dynamicCoverNodeHeightOffset;
        //int nodesFound = 0;			
        Vector3 hidingPosCheckingNow;
        int x = 0;
        //int y;			
        float currDistTarget;

        Vector3 coverHidePos = Vector3.zero;
        Vector3 coverFirePos = Vector3.zero;

        float closestDistToMeSoFarSqr = dynamicCoverMaxDistFromMe * dynamicCoverMaxDistFromMe;
        float distBetweenMeAndCoverNow;

        bool shouldCont = true;

        //Use each vertex on the navmesh as a potential "firing position"
        //Then test whether we can hide from enemy fire by either crouching or moving off to the side (distance to move is hideOffset)
        //If we can see the enemy from the firing position and not see them from the hiding position, then it is a valid cover spot.

        for (int i = 0; i < verts.Length; i++)
        {
            //random value to make sure we don't take the same cover every time
            if (Random.value > 0.5 && Vector3.SqrMagnitude(verts[i] - myPos) > minDistBetweenLastCover)
            {
                currDistTarget = Vector3.SqrMagnitude(verts[i] - targetTransformPos);
                distBetweenMeAndCoverNow = Vector3.SqrMagnitude(verts[i] - myPos);

                if (distBetweenMeAndCoverNow < closestDistToMeSoFarSqr && currDistTarget > minCoverDistSqrd &&
                    currDistTarget < maxCoverDistSqrd)
                {
                    verts[i].y += dynamicCoverNodeFireOffset;

                    //If we can fire from here		
                    if (!Physics.Linecast(verts[i], targetTransformPos, layerMask))
                    {
                        verts[i].y -= hideOffset;
                        //Debug.Break();


                        if (Physics.Raycast(verts[i], targetTransformPos - verts[i], maxDistBehindDynamicCover,
                                layerMask) && !AIController.Instance
                                .IsDynamicCoverSpotCurrentlyUsed(verts[i]))
                        {
                            shouldCont = true;

                            //If chest high wall
                            hidingPosCheckingNow = verts[i];
                            //Check to make sure we have clear LoS between the firing position and the move position.
                            if (!Physics.Linecast(targetTransformPos, verts[i], layerMask) &&
                                Physics.Linecast(hidingPosCheckingNow, targetTransformPos, layerMask))
                            {
                                closestDistToMeSoFarSqr = distBetweenMeAndCoverNow;
                                coverHidePos = hidingPosCheckingNow;
                                coverFirePos = verts[i];
                                shouldCont = false;
                                if (useFirstDynamicCoverFound)
                                {
                                    break;
                                }
                            }

                            //Check for side cover
                            if (shouldCont)
                            {
                                for (x = -1; x <= 1; x += 2)
                                {
                                    hidingPosCheckingNow =
                                        verts[i] + myTransform.right * x * dynamicCoverWidthNeededToHide;
                                    //If we're safe
                                    if (!Physics.Linecast(hidingPosCheckingNow, verts[i], layerMask) &&
                                        Physics.Linecast(hidingPosCheckingNow, targetTransformPos, layerMask))
                                    {
                                        //lastCoverPos = hidingPosCheckingNow;
                                        //return new TacticalAI.CoverData(true, hidingPosCheckingNow, verts[i], true, null);
                                        closestDistToMeSoFarSqr = distBetweenMeAndCoverNow;
                                        coverHidePos = hidingPosCheckingNow;
                                        coverFirePos = verts[i];
                                        shouldCont = false;
                                        if (useFirstDynamicCoverFound)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (coverHidePos != Vector3.zero)
        {
            lastCoverPos = coverHidePos;
            return new CoverData(true, coverHidePos, coverFirePos, true, null);
        }

        return new CoverData();
    }

    CoverData FindRaycastDynamicCover(Vector3 targetTransformPos, Transform transformToDefend)
    {
        if (!transformToDefend)
        {
            transformToDefend = myTransform;
        }

        int i = 0;
        Vector3 currentCastPos = transformToDefend.position;
        Vector3 currentCastVector = Vector3.Normalize(targetTransformPos - transformToDefend.position);
        currentCastVector = Quaternion.Euler(0, -90, 0) * currentCastVector;

        Vector3 hidingPosCheckingNow;
        Vector3 coverFirePos;
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(transformToDefend.position, Vector3.down, out hit, maxRayDist, layerMask))
        {
            currentCastPos = hit.point;
        }

        currentCastPos.y += rayCastHeightOffGround;

        //Debug.Break();
        //Fill a list with potential nodes
        for (i = 0; i < 360f; i += angleBetweenCasts)
        {
            if (Physics.Raycast(currentCastPos, currentCastVector, out hit, maxRayDist, layerMask))
            {
                if (!AIController.Instance.IsDynamicCoverSpotCurrentlyUsed(hit.point))
                {
                    hidingPosCheckingNow = hit.point;
                    hidingPosCheckingNow -= currentCastVector * distFromWallToBe;
                    //
                    //Debug.DrawLine(currentCastPos, hidingPosCheckingNow, Color.yellow);
                    //
                    if (Physics.Linecast(hidingPosCheckingNow, targetTransformPos, layerMask))
                    {
                        coverFirePos = hidingPosCheckingNow + new Vector3(0, dynamicCoverNodeFireOffset, 0);
                        //
                        //Debug.DrawLine(hidingPosCheckingNow, coverFirePos, Color.blue);
                        //
                        if (!Physics.Linecast(coverFirePos, targetTransformPos, layerMask))
                        {
                            //
                            //Debug.DrawLine(coverFirePos, targetTransformPos, Color.green);
                            //
                            lastCoverPos = hidingPosCheckingNow;
                            return new CoverData(true, hidingPosCheckingNow, coverFirePos, true, null);
                        }
                    }
                }
            }

            currentCastVector = Quaternion.Euler(0, angleBetweenCasts, 0) * currentCastVector;
        }


        return new CoverData();
    }
}

public class CoverData
{
    public bool foundCover;
    public Vector3 hidingPosition;
    public Vector3 firingPosition;
    public bool isDynamicCover;
    public CoverNode coverNode;

    public CoverData(bool foundCover, Vector3 hidingPosition, Vector3 firingPosition, bool isDynamicCover,
        CoverNode coverNode)
    {
        this.foundCover = foundCover;
        this.hidingPosition = hidingPosition;
        this.firingPosition = firingPosition;
        this.isDynamicCover = isDynamicCover;
        this.coverNode = coverNode;
    }


    public CoverData()
    {
        foundCover = false;
    }
}