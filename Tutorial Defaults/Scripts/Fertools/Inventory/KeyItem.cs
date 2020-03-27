using System.Collections;
using System.Collections.Generic;
using Fertools.Inventory;
using UnityEngine;

namespace Fertools.Inventory
{
    
    public enum KeyType
    {
        Blue, 
        Red
    }
    
    [CreateAssetMenu(menuName = "Inventory/Key")]
    public class KeyItem : Item
    {
        public override void Use()
        {
            
        }
    }
}
