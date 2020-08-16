using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshInterface : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; }
    private Vector3 lastPos;
    private Vector3 returnVelocity;
    private Transform mTransform;


    public void Init(GameObject gameObject, AgentStats agentStats)
    {
        mTransform = transform;
        lastPos = mTransform.position;

        agent = gameObject.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("No Agent Found!");
        }
        else
        {
            SetSpeed(agentStats.idleSpeed);
        }
    }

    private void Update()
    {
        if (Time.timeScale > 0)
        {
            var position = mTransform.position;
            returnVelocity = (position - lastPos) / Time.deltaTime;
            lastPos = position;
        }
    }

    public virtual void SetDestination(Vector3 v)
    {
        if (agent.enabled)
            agent.SetDestination(v);
        Debug.DrawLine(transform.position, v, Color.red);
        //Debug.Break();
    }

    public virtual bool ReachedDestination()
    {
        return (agent.enabled && !float.IsPositiveInfinity(agent.remainingDistance) && agent.remainingDistance <= 0 &&
                !agent.pathPending);
    }

    public virtual bool PathPartial()
    {
        return (agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial);
    }

    public virtual Vector3 GetDesiredVelocity()
    {
        //return agent.desiredVelocity;
        //return valToReturn;
        return returnVelocity;
    }

    public virtual bool PathPending()
    {
        return agent.pathPending;
    }

    public virtual bool HasPath()
    {
        return agent.hasPath;
    }


    public virtual void SetSpeed(float f)
    {
        agent.speed = f;
    }

    public virtual float GetSpeed()
    {
        return agent.speed;
    }

    public virtual void SetAcceleration(float f)
    {
        agent.acceleration = f;
    }

    public virtual float GetAcceleration()
    {
        return agent.acceleration;
    }

    public virtual void SetStoppingDistance(float f)
    {
        agent.stoppingDistance = f;
    }

    public virtual float GetStoppingDistance()
    {
        return agent.stoppingDistance;
    }


    public virtual float GetRemainingDistance()
    {
        if (agent.enabled)
            return agent.remainingDistance;
        return 0;
    }


    //Turn off the agent component (for stagger) 
    public virtual void DisableAgent()
    {
        agent.enabled = false;
    }

    //Turn on the agent (after stagger)
    public virtual void EnableAgent()
    {
        agent.enabled = true;
    }
    
    public virtual Vector3[] GetNavmeshVertices(){
        return NavMesh.CalculateTriangulation().vertices;
    }
    
}