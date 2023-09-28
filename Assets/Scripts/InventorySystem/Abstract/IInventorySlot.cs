using InventorySystem.Model.Items;

namespace InventorySystem.Abstract
{
    public interface IInventorySlot
    {
        bool IsFull { get; }
        bool IsEmpty { get; }

        IInventoryItem Item { get; }
        TypeItem ItemType { get; }
        int Amount { get; }
        int Capacity { get; }

        void SetItem(IInventoryItem item);
        void Clear();
    }
}