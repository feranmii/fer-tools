using System;
using System.Collections;
using System.Collections.Generic;
using Fertools.Utils;
using UnityEngine;

namespace Fertools.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/Data Container")]
    public class ItemsDataContainer : ScriptableObject
    {
        public List<Item> items;

        private void OnValidate()
        {
            items = FindAssetsByType.FindAssetsOfType<Item>();
        }
    }
}