using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventCallbacks;
using UnityEngine;

public class AIController : Singleton<AIController>
{
    public List<TargetObject> currentTargets = new List<TargetObject>();

    //This is used to assign the IDs to the agents
    private int currentID;
    public LayerMask layerMask;
    public bool usePerformanceMode;

    public float agentCycleTime = .2f;

    private CoverNode[] coverNodes;

    //Dynamic cover
    List<Vector3> currentDynamicCoverSpots = new List<Vector3>();
    [Header("Dynamic Cover")] public float minDistForDynamicCoverSimilarity = 3;


    protected override void Awake()
    {
        base.Awake();

        coverNodes = FindObjectsOfType<CoverNode>();
    }

    private void OnEnable()
    {
        OnAITargetDestroyed.RegisterListener(ProcessAITargetDestroyed);
    }

    private void OnDisable()
    {
        OnAITargetDestroyed.UnregisterListener(ProcessAITargetDestroyed);
    }


    #region Events

    void ProcessAITargetDestroyed(OnAITargetDestroyed destroyed)
    {
        RemoveTargetFromTargetList(destroyed.uid);
    }

    #endregion


    public int AddTarget(int teamID, Transform transform, AITarget target)
    {
        currentID++;
        var targetToAdd = new TargetObject(currentID, teamID, transform, target);
        currentTargets.Add(targetToAdd);
        UpdateAllAgentEnemyLists();
        return currentID;
    }


    private void RemoveTargetFromTargetList(int uid)
    {
        if (currentTargets.Count <= 0) return;
        for (var i = 0; i < currentTargets.Count; i++)
        {
            if (currentTargets[i].aiTarget.uniqueId != uid) continue;
            currentTargets.RemoveAt(i);
            UpdateAllAgentEnemyLists();
            return;
        }
    }

    public void UpdateAllAgentEnemyLists()
    {
        for (int i = 0; i < currentTargets.Count; i++)
        {
            currentTargets[i].aiTarget.UpdateEnemyAndAllyLists(
                GetCurrentTargetsWithIDs(currentTargets[i].aiTarget.alliedTeamsID),
                GetCurrentTargetsWithIDs(currentTargets[i].aiTarget.enemyTeamsID));
        }
    }

    public TargetObject[] GetCurrentTargetsWithIDs(int[] ids)
    {
        var targets = new List<TargetObject>();
        int x;
        foreach (var t in currentTargets)
        {
            for (x = 0; x < ids.Length; x++)
            {
                //Will not detect any targets with a targetPriority lower than 0
                if (ids[x] == t.teamID && t.aiTarget.targetPriority >= 0)
                {
                    targets.Add(t);
                    break;
                }
            }
        }

        return targets.ToArray();
    }

    public void GetAIWithIDs()
    {
    }

    public void GetTargetWithIDs()
    {
    }


    public void GetCurrentAIsWithinRadius()
    {
    }

    public void GetAllTargets()
    {
    }

    public void ChangeAgentsTeam(int uniqueId, int team, bool shouldUpdateList = true)
    {
        foreach (var target in currentTargets.Where(t => t.uid == uniqueId))
        {
            target.teamID = team;
        }

        if (shouldUpdateList)
        {
            UpdateAllAgentEnemyLists();
        }
    }

    public CoverNode[] GetCovers()
    {
        return coverNodes;
    }

    public bool IsDynamicCoverSpotCurrentlyUsed(Vector3 hitPoint)
    {
        for (int i = 0; i < currentDynamicCoverSpots.Count; i++)
        {
            if (Vector3.SqrMagnitude(hitPoint - currentDynamicCoverSpots[i]) < minDistForDynamicCoverSimilarity)
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveACoverSpot(Vector3 aiAgentCurrentCoverNodeFiringPos)
    {
        currentDynamicCoverSpots.Remove(aiAgentCurrentCoverNodeFiringPos);
    }

    public void AddACoverSpot(Vector3 target)
    {
        currentDynamicCoverSpots.Add(target);
    }
}

[System.Serializable]
public class TargetObject
{
    public int uid;
    public int teamID;
    public Transform transform;
    public AITarget aiTarget;

    public TargetObject(int uid, int teamId, Transform transform, AITarget aiTarget)
    {
        this.uid = uid;
        teamID = teamId;
        this.transform = transform;
        this.aiTarget = aiTarget;
    }
}

public class OnAITargetDestroyed : Event<OnAITargetDestroyed>
{
    public int uid;
}