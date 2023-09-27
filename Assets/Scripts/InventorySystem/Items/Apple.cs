using System;
using InventorySystem.Abstract;

namespace InventorySystem.Items
{
    public class Apple : IInventoryItem
    {
        public bool IsEquipped { get; set; }
        public Type Type => GetType();

        public int MaxItemsInInventorySlot { get; }
        public int Amount { get; set; }

        public Apple(int maxItemsInInventorySlot) => MaxItemsInInventorySlot = maxItemsInInventorySlot;

        public IInventoryItem Clone() => new Apple(MaxItemsInInventorySlot);
    }
}