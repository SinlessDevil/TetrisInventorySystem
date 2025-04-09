using System;
using System.Collections.Generic;
using Code.InventoryModel.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.InventoryModel.Items.Data
{
    [CreateAssetMenu(menuName = "StaticData/Inventory/ItemDropConfig", fileName = "ItemDropDropConfig", order = 500)]
    public class ItemDropData : SerializedScriptableObject
    {
        public int CountItems = 3;
        [FormerlySerializedAs("itemDrops")]
        [Space(5)]
        public List<ItemDrop> ItemDrops = new();
    }
    
    [System.Serializable]
    public class ItemDrop
    {
        public int Weight;
        public InventoryBalance.ItemId ItemId;
    }
}