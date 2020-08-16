using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class AITarget : MonoBehaviour
{
    [Foldout("Team Info")] public int targetPriority = 1;


    [Foldout("Team Info"),] public int teamID;

    [Foldout("Team Info"), ReorderableList]
    public int[] alliedTeamsID;

    [Foldout("Team Info"), ReorderableList]
    public int[] enemyTeamsID;


    //This is what other Agents are going to target
    [Foldout("Target "), InfoBox("This is what other Agents are going to target")]
    public Transform targetTransform;

    [Foldout("Target "),
     InfoBox("The transform that determines where other agents will aim their line of sight checks")]
    public Transform losTarget;


    private TargetObject[] enemyTargets;

    [Foldout("Target Check Params")] public float timeBetweenTargetChecksIfEngaging = 7;
    [Foldout("Target Check Params")] public float timeBetweenTargetChecksIfNotEngaging = 12;
    [Foldout("Target Check Params")] public float LOSCheckRate = 0.5f;

    [Foldout("Target Check Params"), ReadOnly]
    private TargetObject currentEnemyTarget;


    [Foldout("Sight")] public bool debugFov;


    [Foldout("References")] public Transform eyeTransform;
    [Foldout("References")] public AIAgent agent;
    [Foldout("References")] public AgentStats stats;


    //ID
    public int uniqueId { get; private set; }


    //LayerMask
    [HideInInspector] public LayerMask layerMask;

    //Targets
    private List<TargetObject> currentlyNoticedTargets = new List<TargetObject>();
    private List<int> targetIDs = new List<int>();

    //Engaging
    private bool engaging;

    //FOV
    private float effectiveFOV;


    private float maxDistToNoticeTarget;

    //
    private List<Vector3> lastKnownTargetPositions = new List<Vector3>();

    private void Awake()
    {
        if (AIController.Instance == null)
        {
            Debug.LogError("Could not find AIController");
            return;
        }

        agent = GetComponent<AIAgent>();

        layerMask = AIController.Instance.layerMask;

        if (!targetTransform)
        {
            targetTransform = transform;
        }

        if (!losTarget)
        {
            losTarget = targetTransform;
        }

        uniqueId = AIController.Instance.AddTarget(teamID, targetTransform, this);

        if (!eyeTransform)
            eyeTransform = targetTransform;

        effectiveFOV = stats.fieldOfView / 2f;

        maxDistToNoticeTarget = stats.maxDistToNoticeTarget * stats.maxDistToNoticeTarget;

        if (agent)
        {
            agent.SetAITarget(this);
        }
    }

    public void SetNewTeam(int team)
    {
        AIController.Instance.ChangeAgentsTeam(uniqueId, team);
        teamID = team;
    }

    private void Start()
    {
        if (agent == null)
            return;
        StartCoroutine(LosLoop());
        StartCoroutine(TargetSelectionLoop());
    }

    //Line of Sight Loop
    IEnumerator LosLoop()
    {
        yield return new WaitForSeconds(Random.value);

        while (agent.enabled)
        {
            CheckForLOSAwareness(false);
            yield return new WaitForSeconds(LOSCheckRate);
        }
    }

    /// <summary>
    /// Pick Target To Fire or Take Cover From
    /// Update Targets more frequently when engaging 
    /// </summary>
    /// <returns></returns>
    IEnumerator TargetSelectionLoop()
    {
        yield return new WaitForSeconds(Random.value);

        while (agent.enabled)
        {
            if (engaging)
            {
                yield return new WaitForSeconds(timeBetweenTargetChecksIfEngaging);
            }
            else
            {
                yield return new WaitForSeconds(timeBetweenTargetChecksIfNotEngaging);
            }

            ChooseTarget();
        }
    }

    public void UpdateEnemyAndAllyLists(TargetObject[] allies, TargetObject[] enemies)
    {
        if (agent == null) return;
        enemyTargets = enemies;

        //if there is no target then exit the engaging state
        if (enemyTargets.Length <= 0)
        {
            agent.EndEngaging();
            engaging = false;
        }

        var lastTargets = currentlyNoticedTargets.ToArray();

        currentlyNoticedTargets = new List<TargetObject>();

        var previousLastKnownTargetPositions = lastKnownTargetPositions.ToArray();
        lastKnownTargetPositions = new List<Vector3>();

        //Take all the existing targets into a new list
        for (var i = 0; i < lastTargets.Length; i++)
        {
            foreach (var t in enemyTargets)
            {


                if (lastTargets[i].uid != t.uid) continue;


                currentlyNoticedTargets.Add(t);
                lastKnownTargetPositions.Add(previousLastKnownTargetPositions[i]);
                break;
            }
        }

        //Check to see if we can see any target 
        CheckForLOSAwareness(engaging);

        ChooseTarget();
    }

    private void NoticeTarget(TargetObject target)
    {
        var idToAdd = target.uid;

        //Make sure we havent seen the target before
        foreach (var t in targetIDs)
        {
            if (t == idToAdd)
                return;
        }

        lastKnownTargetPositions.Add(target.transform.position);
        currentlyNoticedTargets.Add(target);
        targetIDs.Add(idToAdd);

        ChooseTarget();

        //if we are not engaging in combat start now!
        if (!engaging)
        {
            agent.StartEngaging();
            engaging = true;
        }
    }

    private void CheckIfWeStillHaveAwareness()
    {
        var i = 0;
        for (i = 0; i < currentlyNoticedTargets.Count; i++)
        {
            var enemyTransformCheckingNow = currentlyNoticedTargets[i].transform;
            if (eyeTransform && enemyTransformCheckingNow && !Physics.Linecast(eyeTransform.position,
                    enemyTransformCheckingNow.position, layerMask))
            {
                lastKnownTargetPositions[i] = enemyTransformCheckingNow.position;
            }
            else if (enemyTransformCheckingNow &&
                     Vector3.Distance(enemyTransformCheckingNow.position, lastKnownTargetPositions[i]) >
                     stats.distToLoseAwareness)
            {
                currentlyNoticedTargets.RemoveAt(i);
                lastKnownTargetPositions.RemoveAt(i);
                i -= 1;
            }
        }

        if (currentlyNoticedTargets.Count != 0) return;
        agent.EndEngaging();
        engaging = false;
        currentlyNoticedTargets = new List<TargetObject>();
        lastKnownTargetPositions = new List<Vector3>();
        targetIDs = new List<int>();
    }

    private void ChooseTarget()
    {
        if (eyeTransform != null)
        {
            float currentEnemyScore = 0;
            float enemyScoreCheckingNow = 0;
            var enemyTransformCheckingNow = eyeTransform;

            currentEnemyTarget = null;

            bool foundTargetWithLOS = false;
            int i = 0;

            CheckIfWeStillHaveAwareness();

            for (i = 0; i < currentlyNoticedTargets.Count; i++)
            {
                if (currentlyNoticedTargets[i].transform)
                {
                    enemyTransformCheckingNow = currentlyNoticedTargets[i].transform;

                    //Only add points if we have LoS
                    if (!Physics.Linecast(eyeTransform.position, enemyTransformCheckingNow.position, layerMask))
                    {
                        //Get initial score based on distance
                        enemyScoreCheckingNow =
                            Vector3.SqrMagnitude(enemyTransformCheckingNow.position - targetTransform.position);
                        //enemyScoreCheckingNow = Vector3.Distance(enemyTransformCheckingNow.position, targetObjectTransform.position);

                        //Divide by priority
                        enemyScoreCheckingNow /= (currentlyNoticedTargets[i].aiTarget.targetPriority);

                        //See if this score is low enough to warrent changing target
                        if (enemyScoreCheckingNow < currentEnemyScore || currentEnemyScore == 0 || !foundTargetWithLOS)
                        {
                            currentEnemyTarget = currentlyNoticedTargets[i];
                            currentEnemyScore = enemyScoreCheckingNow;
                            foundTargetWithLOS = true;
                        }
                    }
                    //Settle for targets we can't see, if we have to.
                    else if (!foundTargetWithLOS)
                    {
                        enemyScoreCheckingNow =
                            Vector3.SqrMagnitude(enemyTransformCheckingNow.position - targetTransform.position);
                        if (enemyScoreCheckingNow < currentEnemyScore || currentEnemyScore < 0 || !foundTargetWithLOS)
                        {
                            currentEnemyTarget = currentlyNoticedTargets[i];
                            currentEnemyScore = enemyScoreCheckingNow;
                        }
                    }
                }
            }

            if (currentEnemyTarget != null)
            {
            } //Do Shout

            //If all of the above fails, pick a random target- even if it's one we haven't seen
            if (currentEnemyTarget == null && enemyTargets.Length > 0)
            {
                currentEnemyTarget = enemyTargets[Random.Range(0, enemyTargets.Length - 1)];
            }

            if (currentEnemyTarget != null)
            {
                agent.SetTarget(currentEnemyTarget.transform, currentEnemyTarget.aiTarget.losTarget);
            }

            if (currentEnemyTarget == null)

            {
                agent.RemoveTarget();
            }
        }
    }

    private void CheckForLOSAwareness(bool shouldCheck360Degrees)
    {
        if (enemyTargets != null)
        {
            for (int i = 0; i < enemyTargets.Length; i++)
            {
                var transformsCheck = eyeTransform && enemyTargets[i].transform;
                var angleCheck = (shouldCheck360Degrees || Vector3.Angle(eyeTransform.forward,
                                      enemyTargets[i].transform.position - eyeTransform.position) < effectiveFOV);
                var distanceCheck = Vector3.SqrMagnitude(eyeTransform.position - enemyTargets[i].transform.position) <
                                    maxDistToNoticeTarget;

                //Check for line of sight	
                //Sometimes we may not want to restrict the agent's senses to their field of view.	
                if (transformsCheck && angleCheck && distanceCheck)
                {
                    //(Vector3.Angle(eyeTransform.forward, enemyTargets[i].transform.position - eyeTransform.position));
                    //print(shouldCheck360Degrees);
                    if (Physics.Linecast(eyeTransform.position, enemyTargets[i].transform.position, layerMask))
                    {
                        NoticeTarget(enemyTargets[i]);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugFov)
            return;
        // UnityEngine.Debug.DrawRay(eyeTransform.position, eyeTransform.forward * 20, Color.green,
        //     LOSCheckRate);

        Vector3 tarVec = Quaternion.AngleAxis(effectiveFOV, Vector3.up) * eyeTransform.forward;
        //UnityEngine.Debug.DrawRay(eyeTransform.position, tarVec * 20, Color.green, LOSCheckRate);
        tarVec = Quaternion.AngleAxis(-effectiveFOV, Vector3.up) * eyeTransform.forward;
        //UnityEngine.Debug.DrawRay(eyeTransform.position, tarVec * 20, Color.green, LOSCheckRate);

        Handles.color = Colors.Aquamarine;
        Handles.DrawSolidArc(eyeTransform.position, transform.up, tarVec, stats.fieldOfView,
            stats.maxDistToNoticeTarget);
        //Handles.DrawSolidDisc(transform.position, transform.up, effectiveFOV);
    }

    private void OnDisable()
    {
        if (!Application.isPlaying) return;

        var td = new OnAITargetDestroyed();
        td.FireEvent();
    }
}