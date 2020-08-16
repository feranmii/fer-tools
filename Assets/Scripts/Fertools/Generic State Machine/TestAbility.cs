using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.StateMachine;
using UnityEngine;

[RequireComponent(typeof(TestAbility))]
public class TestAbility : MonoBehaviour
{
    private TestMovement _testMovement;
    protected FStateMachine<CharacterStates.MovementStates> _movement;
    protected FStateMachine<CharacterStates.CharacterConditionns> _conditions;


    private void Start()
    {
        _testMovement = GetComponent<TestMovement>();
        _movement = _testMovement.MovementState;
        _conditions = _testMovement.ConditionState;
        
        _movement.ChangeState(CharacterStates.MovementStates.Running);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            
            _movement.ChangeState(CharacterStates.MovementStates.Walking);
            _conditions.ChangeState(CharacterStates.CharacterConditionns.Normal);
        }
        
        
        print(_movement.CurrentState);
    }
}
