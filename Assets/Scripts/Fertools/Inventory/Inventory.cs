using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventCallbacks;
using UnityEngine;

namespace Fertools.Inventory
{
    public class Inventory : Singleton<Inventory>
    {
        public Dictionary<string, Item> items_dict = new Dictionary<string, Item>();

        //public List<KeyValuePair<string, Item>> items => items_dict.ToList();

        public InventoryDataContainer dataContainer;

        public List<Item> carryingItems = new List<Item>();
        
        private void OnEnable()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            foreach (var item in dataContainer.inventoryItems)
            {
                AddItemToDict(item);
            }
        }
        
        private void AddItemToDict(Item item)
        {
            if (!items_dict.ContainsKey(item.name))
            {
                items_dict.Add(item.name, item);
            }
        }

        private void AddToCarryingItem(String id)
        {
            
        }
        
        public Item GetCarryingItem(string id)
        {
            var it = GetItem(id);

            if (carryingItems.Contains(it))
            {
                print("yeahg");
            }
        }
        public Item GetItem(string id)
        {
            Item retVal; 
            if(items_dict.TryGetValue(id, out retVal))
            {
                //works
            }
            else
            {
                Debug.LogError("Cannot Find Item In the Inventory");
            }

            return retVal;
        }
        
        

    }

    public class OnItemAdded : Event<OnItemAdded>
    {

    }

    public class OnItemRemoved : Event<OnItemRemoved>
    {

    }
}