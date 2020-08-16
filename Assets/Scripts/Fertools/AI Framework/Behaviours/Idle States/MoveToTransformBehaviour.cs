using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTransformBehaviour : BaseState
{
    public MoveToTransformBehaviour(AIAgent aiAgent) : base(aiAgent)
    {
    }


    public override void Execute()
    {
        if (aiAgent.keyTransform)
        {
            targetVector = aiAgent.keyTransform.position;
        }
    }
}