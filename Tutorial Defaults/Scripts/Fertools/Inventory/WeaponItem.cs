using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fertools.Inventory
{

    [CreateAssetMenu(menuName = "Inventory/Weapon")]
    public class WeaponItem : Item
    {
        public int magazineSize;
        public int bulletCount;
        
        public override void Use()
        {
            
            Debug.Log("Blam Blam");
        }
    }
}