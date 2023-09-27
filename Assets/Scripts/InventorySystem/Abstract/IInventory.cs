using System;

namespace InventorySystem.Abstract
{
    public interface IInventory
    {
        int Capacity { get; set; }
        bool IsFull { get; }

        IInventoryItem GetItem(Type itemType);
        IInventoryItem[] GetAllItems();
        IInventoryItem[] GetAllItems(Type itemType);
        IInventoryItem[] GetEquippedItems();
        IInventorySlot[] GetAllSlots(Type itemType);
        IInventorySlot[] GetAllSlots();

        int GetItemAmount(Type itemType);

        bool TryToAdd(object sender, IInventoryItem item);
        void Remove(object sender, Type itemType, int amount = 1);
        bool HasItem(Type type, out IInventoryItem item);
    }
}