using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BaseState : IState
{
    protected AIAgent aiAgent;
    protected AICoverFinder aiCoverFinder;
    protected AIShooting aiShooting;
    
    protected NavmeshInterface navInterface;
    protected LayerMask layerMask;
    public Transform transform;
    public Vector3 targetVector;


    protected BaseState(AIAgent aiAgent)
    {
        this.aiAgent = aiAgent;
        aiCoverFinder = aiAgent.coverFinder;
        aiShooting = aiAgent.aiShooting;
        navInterface = aiAgent.navInterface;
        layerMask = aiAgent.layerMask;
        transform = aiAgent.transform;
    }


    public virtual void Enter()
    {
    }

    public virtual void Execute()
    {
    }

    public virtual void Tick()
    {
        
    }
    public virtual void Exit()
    {
    }
}
