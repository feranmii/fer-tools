using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fertools.Inventory
{

    public enum ItemType
    {
        Gun,
        Throwable
    }
    

    public abstract class Item : ScriptableObject
    {
        public bool isUnlocked;
        public int cost;
        
        public ItemType itemType;

        public void UnlockItem()
        {
            isUnlocked = true;
        }

        public void LockItem()
        {
            
        }
        
        public abstract void Use();

        public void Drop()
        {
            
        }

    }
}