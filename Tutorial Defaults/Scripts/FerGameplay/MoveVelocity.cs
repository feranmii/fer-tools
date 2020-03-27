using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FerGameplay.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveVelocity : MonoBehaviour, IVelocity
    {
        public PlayerStats stats;
        private Rigidbody rb;
        private Vector3 _velocityVector;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void SetVelocity(Vector3 vel)
        {
            _velocityVector = vel;
        }
        public void AddVelocity(Vector3 vel)
        {
            _velocityVector += vel;
        }
        private void FixedUpdate()
        {
            rb.velocity = _velocityVector * stats.moveSpeed;
        }
        
    }
}