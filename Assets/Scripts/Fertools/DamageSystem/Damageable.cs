using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Damageable : MonoBehaviour
{
    public int maxHitPoints;

    public float invulnerabilityTime;

    [Tooltip(
        "The angle from the which that damageable is hitable. Always in the world XZ plane, with the forward being rotate by hitForwardRoation")]
    [Range(0.0f, 360.0f)]
    public float hitAngle = 360.0f;

    [Tooltip("Allow to rotate the world forward vector of the damageable used to define the hitAngle zone")]
    [Range(0.0f, 360.0f)]
    public float hitForwardRotation = 360.0f;


    public bool isInvulnerable { get; set; }

    public int currentHitPoints { get; private set; }

    public UnityEvent OnDeath, OnReceiveDamage, OnHitWhileInvulnerable, OnBecomeVulnerable, OnResetDamage;

    protected float m_TimeSinceLastHit = 0.0f;
    protected Collider m_Collider;

    private System.Action schedule;

    private void Start()
    {
        ResetDamage();
        m_Collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isInvulnerable)
        {
            m_TimeSinceLastHit += Time.deltaTime;

            if (m_TimeSinceLastHit > invulnerabilityTime)
            {
                m_TimeSinceLastHit = 0.0f;
                isInvulnerable = false;
                OnBecomeVulnerable.Invoke();
            }
        }
    }

    public void ResetDamage()
    {
        currentHitPoints = maxHitPoints;
        isInvulnerable = false;
        m_TimeSinceLastHit = 0.0f;
        OnResetDamage.Invoke();
    }

    public void SetColliderState(bool enabled)
    {
        m_Collider.enabled = enabled;
    }

    public void ApplyDamage(DamageInfo info)
    {
        if (currentHitPoints <= 0)
        {
            return;
        }

        if (isInvulnerable)
        {
            OnHitWhileInvulnerable.Invoke();
            return;
        }

        var forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        Vector3 positionToDamager = info.damageSource - transform.position;
        positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);

        if (Vector3.Angle(forward, positionToDamager) > hitAngle * 0.5f)
            return;

        isInvulnerable = true;
        currentHitPoints -= info.amount;
        
        if (currentHitPoints <= 0)
        {
            schedule += OnDeath.Invoke;
        }
        else
        {
            OnReceiveDamage.Invoke();
        }
        

    }

    private void LateUpdate()
    {
        if (schedule != null)
        {
            schedule();
            schedule = null;
        }
    }
    
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 forward = transform.forward;
        forward = Quaternion.AngleAxis(hitForwardRotation, transform.up) * forward;

        if (Event.current.type == EventType.Repaint)
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f,
                EventType.Repaint);
        }


        UnityEditor.Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        forward = Quaternion.AngleAxis(-hitAngle * 0.5f, transform.up) * forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, forward, hitAngle, 1.0f);
    }
#endif
}