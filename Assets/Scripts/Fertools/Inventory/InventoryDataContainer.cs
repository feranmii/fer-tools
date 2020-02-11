using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Utils;
using UnityEngine;

namespace Fertools.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Data Container")]
    public class InventoryDataContainer : ScriptableObject
    {
        public List<Item> inventoryItems;

        private void OnValidate()
        {
            inventoryItems = FindAssetsByType.FindAssetsOfType<Item>();
        }
    }
}