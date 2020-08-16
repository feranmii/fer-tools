using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    AIStateMachine stateMachine = new AIStateMachine();


    #region Public Variables

    //References

    [Foldout("Patrol Params"), ShowIf("IsPatrol"), ReorderableList]
    public Transform[] patrolNodes;

    [Foldout("Patrol Params"), ShowIf("IsPatrol")]
    public bool showPatrolPath;

    [Foldout("Patrol Params"), ShowIf("IsPatrol")]
    public bool randomizePatrol;

    [Foldout("Patrol Params"), ShowIf("IsPatrol")]
    public Color patrolPathColor;


    //Cover 
    [Foldout("Cover"), ReadOnly, ShowIf("IsTactical")]
    public bool inCover = false;

    [Foldout("Cover"), ReadOnly, ShowIf("IsTactical")]
    public CoverNode currentCoverNode;

    [Foldout("Cover"), ShowIf("IsTactical")]
    public bool shouldFireFromCover = true;


    [Space] [Foldout("References")] public AgentStats agentStats;

    //This is a point of reference for some of the agent's actions
    [Foldout("References")] public Transform keyTransform;


    [Foldout("Debug"), ReadOnly] public string Debug_CurrentState;
    [Foldout("Debug"), ReadOnly] public string Debug_PreviousState;

    #endregion


    #region Private Variables

    public Transform targetTransform;
    private bool isEngaging;
    private bool isSprinting;


    [HideInInspector] public Transform mTransform;
    private Vector3 lastVector;
    [HideInInspector] public LayerMask layerMask;

    //States 
    private BaseState idleState;
    private BaseState combatState;

    //Target
    private AITarget aiTarget;


    //Cover
    [HideInInspector] public Vector3 currentCoverNodePos;
    [HideInInspector] public Vector3 currentCoverNodeFiringPos;
    private bool foundDynamicCover = false;
    public bool useAdvancedCover = false;


    //References

    [HideInInspector] public AIShooting aiShooting;
    [HideInInspector] public NavmeshInterface navInterface;
    [HideInInspector] public AICoverFinder coverFinder;
    [HideInInspector] public float origAgentStoppingDist;

    #endregion

    #region Editor Check

    private bool IsPatrol()
    {
        return agentStats.idleBehaviour == IdleBehaviour.Patrol;
    }

    private bool IsTactical()
    {
        return agentStats.AIType == AIType.Tactical;
    }

    #endregion

    private void Awake()
    {
        aiShooting = GetComponent<AIShooting>();
        coverFinder = GetComponent<AICoverFinder>();

        mTransform = transform;

        navInterface = gameObject.AddComponent<NavmeshInterface>();
        navInterface.Init(gameObject, agentStats);
    }

    private void Start()
    {
        //stateMachine.ChangeState(new PatrolState());

        //Get Default Behaviour
        GetDefaultBehaviours();
        layerMask = AIController.Instance.layerMask;

        //Test

        StartCoroutine(AICycle());
    }

    private IEnumerator AICycle()
    {
        while (enabled)
        {
            stateMachine?.Update();
            // stateMachine?.SetStateTargetVector(AIBlackboard.Instance.player.transform.position);

            MoveAI();

            if (!targetTransform && !isEngaging)
            {
                isSprinting = false;
                //Stop Sprinting Animation
            }
            else if (Vector3.SqrMagnitude(mTransform.position - stateMachine.TargetVector) <
                     agentStats.distanceFromTargetToSprint && isEngaging)
            {
                //Stop Sprinting Animation

                AdjustSpeed();
            }

            yield return new WaitForSeconds(AIController.Instance.agentCycleTime);
        }
    }


    private void MoveAI()
    {
        if (this.enabled && stateMachine != null && stateMachine.TargetVector != lastVector)
        {
            navInterface.SetDestination(stateMachine.TargetVector);
        }

        if (stateMachine != null) lastVector = stateMachine.TargetVector;
    }

    private void AdjustSpeed()
    {
        if (navInterface == null)
            return;


        navInterface.SetSpeed(isEngaging ? agentStats.runSpeed : agentStats.idleSpeed);
    }

    public void SetTarget(Transform curEnemyTransform, Transform losTargetTransform)
    {
        targetTransform = curEnemyTransform;
        //Head look

        //Give gun target
        if (aiShooting)
        {
            aiShooting.AssignTarget(targetTransform, losTargetTransform);
        }
    }

    public void RemoveTarget()
    {
        targetTransform = null;
    }

    private void Update()
    {
        stateMachine?.Tick();
#if UNITY_EDITOR
        Debug_CurrentState = stateMachine.currentState?.ToString();
        Debug_PreviousState = stateMachine.previousState?.ToString();
#endif

        // currentBehaviour.Update();
    }

    //This will vary based on the Idle behaviour selected

    private BaseState GetIdleBehaviour()
    {
        switch (agentStats.idleBehaviour)
        {
            case IdleBehaviour.Patrol:
                return GetBehaviour(AIBehaviour.Patrolling);
            case IdleBehaviour.Wander:
                return GetBehaviour(AIBehaviour.Wander);
            case IdleBehaviour.Search:
                return GetBehaviour(AIBehaviour.Chase);
                break;
            case IdleBehaviour.MoveToTransform:

                return GetBehaviour(AIBehaviour.MoveToTransform);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //This will vary based on the AI Type selected
    private BaseState GetCombatBehaviour()
    {
        switch (agentStats.AIType)
        {
            case AIType.Berserker:
                return GetBehaviour(AIBehaviour.ChargeTarget);
                break;
            case AIType.Tactical:
                return GetBehaviour(AIBehaviour.Cover);
                break;
            case AIType.Skirmish:
                return GetBehaviour(AIBehaviour.Skirmish);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void GetDefaultBehaviours()
    {
        idleState = GetIdleBehaviour();
        combatState = GetCombatBehaviour();
        // stateMachine.ChangeState(idleState);
        SetBehaviour();
    }

    void SetBehaviour()
    {
        AdjustSpeed();

        if (isEngaging && combatState != null)
        {
            stateMachine.ChangeState(combatState);
        }
        else
        {
            stateMachine.ChangeState(idleState);
        }
    }

    private BaseState GetBehaviour(AIBehaviour behaviour)
    {
        BaseState retVal = null;

        switch (behaviour)
        {
            case AIBehaviour.Patrolling:
                retVal = new PatrolBehaviour(this);
                break;
            case AIBehaviour.Wander:
                retVal = new WanderBehaviour(this);
                break;
            case AIBehaviour.Skirmish:
                retVal = new SkirmishBehaviour(this);
                break;
            case AIBehaviour.ChargeTarget:
                retVal = new BerserkerBehaviour(this);
                break;
            case AIBehaviour.Cover:
                retVal = new TacticalBehaviour(this);
                break;
            case AIBehaviour.MoveToTransform:
                retVal = new MoveToTransformBehaviour(this);
                break;
            case AIBehaviour.Chase:
                retVal = new ChaseState(this);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(behaviour), behaviour, null);
        }

        retVal.Enter();

        return retVal;
    }


    public void StartEngaging()
    {
        isEngaging = true;

        SetBehaviour();

        navInterface.SetSpeed(agentStats.runSpeed);
    }


    public void EndEngaging()
    {
        isEngaging = false;

        SetBehaviour();

        //    navInterface.SetSpeed(agentStats.runSpeed);
    }


    private void OnDrawGizmos()
    {
        if (showPatrolPath && patrolNodes.Length > 1)
        {
            for (int i = 1; i < patrolNodes.Length; i++)
            {
                if (patrolNodes[i])
                {
                    Handles.color = patrolPathColor;
                    Handles.DrawLine(patrolNodes[i].position, patrolNodes[i - 1].position);
                }
            }
        }
    }

    public void SetAITarget(AITarget aiTarget)
    {
        this.aiTarget = aiTarget;
    }

    public int[] GetEnemyTeamIDs()
    {
        return GetEnemyIDsFromTargets();
    }

    private int[] GetEnemyIDsFromTargets()
    {
        if (aiTarget)
        {
            return aiTarget.enemyTeamsID;
        }

        return null;
    }

    public bool IsEnaging()
    {
        return isEngaging;
    }

    public void ShouldFireFromCover(bool b)
    {
        shouldFireFromCover = b;
    }


    public bool HaveCover()
    {
        return (currentCoverNode != null || foundDynamicCover);
    }

    public void SetOrigStoppingDistance()
    {
        if (navInterface)
            navInterface.SetStoppingDistance(origAgentStoppingDist);
    }

    public Vector3 GetEyePos()
    {
        return aiTarget.eyeTransform.position;
    }
}

#region Enums

public enum AIType
{
    Berserker,
    Tactical,
    Skirmish
}

public enum IdleBehaviour
{
    Patrol,
    Wander,
    Search,
    MoveToTransform
}

public enum AIBehaviour
{
    Patrolling,
    Wander,
    Skirmish,
    ChargeTarget,
    Cover,
    MoveToTransform,
    Chase
}

#endregion