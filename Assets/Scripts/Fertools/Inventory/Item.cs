using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fertools.Inventory
{

    public enum ItemType
    {
        OneTimeUse,
        Reusable
    }
    

    public abstract class Item : ScriptableObject
    {
        public ItemType itemType;
        public abstract void Use();

        public void Drop()
        {
            
        }

    }
}