using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Gun/Gun Data")]
public class GunData : ScriptableObject
{
    public enum WeaponType
    {
        Hitscan, 
        ProjectileBased
    }

    public WeaponType weaponType;
    public bool limitedBullets;
    
    [BoxGroup("Gun")] public float fireRate;
    [BoxGroup("Gun")] public float shootDistance;
    
    [BoxGroup("Bullets"), ShowIf("limitedBullets")]
    public int magazineBullet;
    [BoxGroup("Bullets"), ShowIf("limitedBullets")]
    public int ammo;

    [BoxGroup("Bullets"), ShowIf("limitedBullets")]
    public IntVariable currentAmmo;
    
    [BoxGroup("Bullets"), ShowIf("limitedBullets")]
    public IntVariable carryingAmmo;
 
    
    bool IsHitscan()
    {
        return weaponType == WeaponType.Hitscan;
    }
}
