using System;
using System.Collections.Generic;
using Code.Inventory.Items.Data;

namespace Code.Inventory
{
    public interface IInventory
    {
        event Action<InventoryActionData> OnItemAdded;
        event Action<InventoryActionData> OnItemRemoved;
        List<Item> Items { get; }
        List<GridCell> Cells { get; }
        PlaceTestResult CanPlace(GridCell targetCell, Item item, bool ignoreItself);
        bool TryRemove(Item item, out GridCell gridCell);
        bool TryAdd(GridCell cell, Item item);
        Item GetById(string itemId);
        bool TryAdd(Item item);
        int GridIndex(GridCell cell);
        int GridIndex(int x, int y);
    }
}