using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public interface IState
{
    void Enter();
    //Runs every frame
    void Tick();

    //Runs Every AI Cycle
    void Execute();
    void Exit();
}

public class AIStateMachine
{
    public IState currentState;
    public IState previousState;

    public Action onStateChanged;


    public Vector3 TargetVector
    {
        get
        {
            var state = (BaseState) currentState;

            return state.targetVector;
        }
    }

    public void SetStateTargetVector(Vector3 target)
    {
        var state = (BaseState) currentState;

        state.targetVector = target;
    }


    public void ChangeState(IState newState)
    {
        if (currentState != newState)
        {
            onStateChanged?.Invoke();
        }

        previousState = currentState;

        currentState?.Exit();


        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Execute();
    }

    public void Tick()
    {
        currentState?.Tick();
    }
}