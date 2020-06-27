using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Damageable : MonoBehaviour
{
    public struct DamageInfo
    {
        public MonoBehaviour damager;
        public Vector3 direction;
        public Vector3 damageSource;
        public int amount;
    }
}

public enum DamageMessageType
{
    DAMAGED,
    DEAD,
    RESPAWN
}