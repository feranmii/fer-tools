using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Agents/Agent Stats")]
public class AgentStats : ScriptableObject
{
    //Behaviour
    public AIType AIType;
    public IdleBehaviour idleBehaviour = IdleBehaviour.Wander;


    [Space] [Foldout("Movement")] public float sprintSpeed = 6f;
    [Foldout("Movement")] public float runSpeed = 5f;
    [Foldout("Movement")] public float alertSpeed = 4f;
    [Foldout("Movement")] public float idleSpeed = 3f;
    [Foldout("Movement")] public bool canSprint;

    [Foldout("Movement"), ShowIf("canSprint")]
    public float distanceFromTargetToSprint = 25f;

    [Space] [Foldout("Sight")] public float fieldOfView = 130;

    [Foldout("Sight")] public float maxDistToNoticeTarget = 9999f;

    [Foldout("Sense")] public float distToLoseAwareness = 35f;

    //Wander Params
    [Foldout("Wander Params"), ShowIf("IsWander"), MinMaxSlider(5, 50)]
    public Vector2 wanderRadius;

    [Foldout("Wander Params"), ShowIf("IsWander"), Range(1, 6)]
    public float distanceToChooseNewWanderPoint;

    [Space]
    //Patrol Params
    [Foldout("Patrol Params"), Range(1, 6), ShowIf("IsPatrol"),
     InfoBox("How close the agent would come to the patrol node before picking another one")]
    public float stopDistanceToPatrolPoint;

    [Foldout("Patrol Params"), ShowIf("IsPatrol")]
    public bool waitAtPatrolNode;

    [Foldout("Patrol Params"), Range(1, 6), ShowIf(EConditionOperator.And, "IsPatrol", "waitAtPatrolNode")]
    public float waitTimeAtPatrolNode;

    [Foldout("Search Params"), ShowIf("IsSearch")]
    public float radiusToCallOfSearch = 5f;


    [Foldout("Skirmish Params"), ShowIf("IsSkirmish")]
    public float minSkirmishDistFromTarget = 7f;

    [Foldout("Skirmish Params"), ShowIf("IsSkirmish")]
    public float maxSkirmishDistFromTarget = 20f;

    [Foldout("Skirmish Params"), ShowIf("IsSkirmish")]
    public bool canCrossBehindTarget = true;

    [Foldout("Skirmish Params"), ShowIf("IsSkirmish")]
    public float maxTimeToWaitAtEachSkirmishPoint = 3f;


    [Foldout("Cover"), ShowIf("IsTactical")]
    public float timeBetweenSafetyChecks = 1.0f;

    [Foldout("Cover"), ShowIf("IsTactical")]
    public float maxTimeInCover = 10f;

    [Foldout("Cover"), ShowIf("IsTactical")]
    public float minTimeInCover = 5f;

    [Foldout("Cover"), ShowIf("IsTactical")]
    public float minDistToTargetIfNotInCover = 5f;


    [Foldout("Dodging")] public float dodgingClearHeight = 1f;

    [Space(20)] [Foldout("Shooting Params"), Header("Shooting Rate")]
    public float minPauseTime = 1;

    [Foldout("Shooting Params")] public float randomPauseTimeAdd = 2;
    [Foldout("Shooting Params")] public int minRoundsPerVolley = 1;
    [Foldout("Shooting Params")] public int maxRoundsPerVolley = 10;
    [Foldout("Shooting Params")] public float minimumDistToFireGun = 0;
    [Foldout("Shooting Params")] public float maximumDistToFireGun = 9999;

    [Foldout("Shooting Params")] public float rateOfFire = 2;
    [Foldout("Shooting Params")] public float burstFireRate = 12;
    [Foldout("Shooting Params")] public int shotsPerBurst = 1;

    [Foldout("Shooting Params"), Space, Header("Reloading")]
    public int bulletsUntilReload = 60;

    [Foldout("Shooting Params"),] public float reloadTime = 2;

    [Space] [Foldout("Shooting Params"), Header("Accuracy")]
    public float inaccuracy = 1;

    [Foldout("Shooting Params")] [Range(0.0f, 90.0f)]
    public float maxFiringAngle = 10;

    [Foldout("Shooting Params")] [Range(0.0f, 90.0f)]
    public float maxSecondaryFireAngle = 40;


    #region Editor Checks

    private bool IsPatrol()
    {
        return idleBehaviour == IdleBehaviour.Patrol;
    }

    private bool IsSearch()
    {
        return idleBehaviour == IdleBehaviour.Search;
    }

    private bool IsWander()
    {
        return idleBehaviour == IdleBehaviour.Wander;
    }

    private bool IsTactical()
    {
        return AIType == AIType.Tactical;
    }

    private bool IsSkirmish()
    {
        return AIType == AIType.Skirmish;
    }

    private bool IsBerserker()
    {
        return AIType == AIType.Berserker;
    }

    #endregion


    private void OnValidate()
    {
        if (idleSpeed > runSpeed)
            idleSpeed = runSpeed;

        if (distanceToChooseNewWanderPoint > wanderRadius.x)
        {
            distanceToChooseNewWanderPoint = wanderRadius.x;
        }
    }
}