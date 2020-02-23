
using System;
using Fertools.Inventory;
using NaughtyAttributes;
using UnityEngine;


namespace Fertools.Weapons.Guns
{
    [CreateAssetMenu(menuName = "Weapons/Gun/Gun Item")]
    public class Gun : Item
    {
        
        public GunData gunData;
        private bool _canShoot;
        private float _t;
        private bool _hasInit;

        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            gunData.currentAmmo.value = gunData.magazineBullet;
            gunData.carryingAmmo.value = gunData.ammo;
        }

        public override void Use()
        {
            if (gunData.limitedBullets) 
            {
                if(gunData.currentAmmo.value > 0)
                {
                    RemoveBullets(1); //TODO: Change this to depend on the bulletcost
                }
                else
                {
                    if (gunData.carryingAmmo.value > gunData.magazineBullet)
                    {
                        var diff = gunData.magazineBullet - gunData.currentAmmo.value;
                        Reload(gunData.magazineBullet);
                    }
                    else
                    {
                        if(gunData.carryingAmmo.value > 0)
                            Reload(gunData.carryingAmmo.value);
                    }
                }
            }
        }

        public void RemoveBullets(int value)
        {
            gunData.currentAmmo.value -= value;
        }

        public void AddBullets(int value)
        {
            gunData.currentAmmo.value += value;

        }

        public void Reload(int bulletsToReload)
        {
            gunData.currentAmmo.value += bulletsToReload;
            gunData.carryingAmmo.value -= bulletsToReload;
        }
    }
  
}
