using System;
using UnityEngine;

namespace Code.InventoryModel.Items.Data
{
    [Serializable]
    public abstract class Item
    {
        [Space(10)] [Header("Base")]
        public string Id;
        public string Name;
        public string Description;
        public int ItemLevel = 1;
        [Space(10)][Header("Addition Component")]
        public InventoryPlacement InventoryPlacement;
        public InventoryGraphic Graphic;
        public InventoryCharacteristics Characteristics;

        [HideInInspector] public Guid InstanceId;
        
        public Item()
        {
            InventoryPlacement = new InventoryPlacement(size: 3);
            Graphic = new InventoryGraphic();
            Characteristics = new InventoryCharacteristics();
        }

        public Item Clone()
        {
            var clone = (Item) MemberwiseClone();
            clone.InstanceId = Guid.NewGuid();
            clone.InventoryPlacement = new InventoryPlacement((bool[,]) InventoryPlacement.Space.Clone());

            return clone;
        }
    }
}