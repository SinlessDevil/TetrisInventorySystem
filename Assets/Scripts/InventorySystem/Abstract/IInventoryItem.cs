using InventorySystem.Model.Items;

namespace InventorySystem.Abstract
{
    public interface IInventoryItem
    {
        TypeItem ItemType { get; }
        IInventoryItemInfo Info { get; }
        IInventoryItemState State { get; }

        IInventoryItem Clone();
    }
}