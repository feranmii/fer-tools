
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
            gunData.currentAmmo.Value = gunData.magazineBullet;
            gunData.carryingAmmo.Value = gunData.ammo;
        }

        public override void Use()
        {
            if (gunData.limitedBullets) 
            {
                if(gunData.currentAmmo.Value > 0)
                {
                    RemoveBullets(1); //TODO: Change this to depend on the bulletcost
                }
                else
                {
                    if (gunData.carryingAmmo.Value > gunData.magazineBullet)
                    {
                        var diff = gunData.magazineBullet - gunData.currentAmmo.Value;
                        Reload(gunData.magazineBullet);
                    }
                    else
                    {
                        if(gunData.carryingAmmo.Value > 0)
                            Reload(gunData.carryingAmmo.Value);
                    }
                }
            }
        }

        public void RemoveBullets(int Value)
        {
            gunData.currentAmmo.Value -= Value;
        }

        public void AddBullets(int Value)
        {
            gunData.currentAmmo.Value += Value;

        }

        public void Reload(int bulletsToReload)
        {
            gunData.currentAmmo.Value += bulletsToReload;
            gunData.carryingAmmo.Value -= bulletsToReload;
        }
    }
  
}
