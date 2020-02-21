using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using Fertools.Inventory;
using UnityEngine;

namespace Fertools.Store
{
    public class Store : MonoBehaviour
    {

        public CashVariable cash;
        
        public Dictionary<string, Item> unlockedItems_dict = new Dictionary<string, Item>();
        public Dictionary<string, Item> lockedItems_dict = new Dictionary<string, Item>();
        public ItemsDataContainer itemsList;


        private void Start()
        {
            Init();
        }

        private void Init()
        {
            foreach (var item in itemsList.items)
            {
                if (item.isUnlocked)
                {
                    if(!unlockedItems_dict.ContainsKey(item.name))
                        unlockedItems_dict.Add(item.name, item);
                }
                else
                {
                    if(!lockedItems_dict.ContainsKey(item.name))
                        lockedItems_dict.Add(item.name, item);
                }
            }
        }

        private void BuyItem(string name)
        {
            var itemToBuy = GetItem(name);

            if (!itemToBuy.isUnlocked)
            {
                var success = cash.AttemptPurchase(itemToBuy.cost);
                if (success)
                {
                    itemToBuy.UnlockItem();
                    
                    var oip = new OnItemPurchased(itemToBuy);
                    oip.FireEvent();
                    
                    Init();
                }
                
            }
            else
            {
                print("Item Is Already Unlocked!");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                BuyItem("M4");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                BuyItem("AK");
            }
            
        }

        private Item GetItem(string name)
        {
            Item retVal;
            
            if (unlockedItems_dict.ContainsKey(name))
            {
                unlockedItems_dict.TryGetValue(name, out retVal);
                return retVal;
            }
            else
            {
                lockedItems_dict.TryGetValue(name, out retVal);
                return retVal;
            }
        }
    }
}


public class OnItemPurchased : Event<OnItemPurchased>
{
    public Item itemPurchased;

    public OnItemPurchased(Item itemPurchased)
    {
        this.itemPurchased = itemPurchased;
    }
}