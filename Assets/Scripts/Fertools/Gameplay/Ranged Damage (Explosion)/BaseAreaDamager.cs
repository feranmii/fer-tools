using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class BaseAreaDamager : MonoBehaviour
{
    [Foldout("Damage Info")] public int maxNumberOfAffected = 10;
    [Foldout("Damage Info"), Min(1)] public int damageRadius = 10;
    [Foldout("Damage Info"),] public bool damagePerDistance = true;

    [Foldout("Damage Info"), Min(1)] public int damageAmount = 4;

    [Foldout("Effects")] public GameObject explosionFX;

    [Foldout("Debug")] public bool debug;
    [Foldout("Debug")] public bool customStartPoint;

    [Foldout("Debug"), ShowIf("customStartPoint")]
    public Transform startPoint;

    private Transform mTransform;

    private void Awake()
    {
        mTransform = transform;
    }

    [Button()]
    public void Explode()
    {
        var cols = new Collider[maxNumberOfAffected];
        var numOfCollisions = Physics.OverlapSphereNonAlloc(mTransform.forward, damageRadius, cols);

        //Deal Damage
        for (var i = 0; i < numOfCollisions; i++)
        {
            var damage = cols[i].GetComponent<Damageable>();
            if (damage != null && damage.gameObject != gameObject)
            {
                var finalDamage = 0;

                if (damagePerDistance)
                {
                    var distance = Vector3.Distance(damage.transform.position, mTransform.position);

                    finalDamage = Mathf.RoundToInt((1 - Mathf.Clamp01(distance / damageRadius)) * damageAmount);
                }
                else
                {
                    finalDamage = damageAmount;
                }

                print(finalDamage);
                var damageInfo = new Damageable.DamageInfo
                {
                    amount = finalDamage,
                    damager = this,
                    damageSource = mTransform.position
                };

                damage.ApplyDamage(damageInfo);
            }
        }

        //Spawn Explosion
        if (explosionFX)
        {
            Instantiate(explosionFX, transform.position, Quaternion.identity);
        }

        //Destroy object
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        if (!debug)
            return;


        var c = Colors.Orangered;
        c.a = (damageAmount / 100f);
        Handles.color = c;
        if (customStartPoint && startPoint != null)
        {
            Handles.DrawSolidDisc(startPoint.position, transform.up, damageRadius);
        }
        else
        {
            Handles.DrawSolidDisc(transform.position, transform.up, damageRadius);
        }
    }
}