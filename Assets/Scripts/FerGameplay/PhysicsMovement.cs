using System;
using System.Collections;
using System.Collections.Generic;
using FerGameplay.Movement;
using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    public PlayerInput _playerInput;
    private MoveVelocity _moveVelocity;

    private void Awake()
    {
        _moveVelocity = GetComponent<MoveVelocity>();
    }

    private void Update()
    {
        _moveVelocity.SetVelocity(_playerInput.normalizedMoveInputVector);
        
        print(Utils.GetMouseWorldPosition(Camera.main));
    }
}
