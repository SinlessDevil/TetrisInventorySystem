using System;

namespace InventorySystem.Abstract
{
    public interface IInventoryItem
    {
        Type Type { get; }

        IInventoryItemInfo Info { get; }
        IInventoryItemState State { get; }

        IInventoryItem Clone();
    }
}