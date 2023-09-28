using InventorySystem.Model.Items;

namespace InventorySystem.Abstract
{
    public interface IInventory
    {
        int Capacity { get; set; }
        bool IsFull { get; }

        IInventoryItem GetItem(TypeItem itemType);
        IInventoryItem[] GetAllItems();
        IInventoryItem[] GetAllItems(TypeItem itemType);
        IInventoryItem[] GetEquippedItems();
        IInventorySlot[] GetAllSlots(TypeItem itemType);
        IInventorySlot[] GetAllSlots();

        int GetItemAmount(TypeItem itemType);

        bool TryToAdd(object sender, IInventoryItem item);
        void Remove(object sender, TypeItem itemType, int amount = 1);
        bool HasItem(TypeItem type, out IInventoryItem item);
    }
}