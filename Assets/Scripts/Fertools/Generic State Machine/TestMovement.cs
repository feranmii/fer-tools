using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.StateMachine;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public FStateMachine<CharacterStates.MovementStates> MovementState;
    public FStateMachine<CharacterStates.CharacterConditionns> ConditionState;


    private void Start()
    {
        MovementState = new FStateMachine<CharacterStates.MovementStates>(gameObject, false);
        ConditionState  = new FStateMachine<CharacterStates.CharacterConditionns>(gameObject, false);
        
        
    }
}
