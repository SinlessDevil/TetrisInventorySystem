using System;
using System.Collections.Generic;
using Code.Inventory;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel.InventoryAddCondition;
using Code.InventoryModel.Items.Data;

namespace Code.InventoryModel
{
    public class ExpandableInventory : IInventory
    {
        private readonly TetrisInventory _inventory;

        public ExpandableInventory(TetrisInventory inventory, IInventoryExpandService expandService)
        {
            _inventory = inventory;
            
            inventory.WithCondition(new CellBoughtCondition(expandService));
        }

        public event Action<InventoryActionData> OnItemAdded
        {
            add => _inventory.OnItemAdded += value;
            remove => _inventory.OnItemAdded -= value;
        }

        public event Action<InventoryActionData> OnItemRemoved
        {
            add => _inventory.OnItemRemoved += value;
            remove => _inventory.OnItemRemoved -= value;
        }

        public List<Item> Items => _inventory.Items;

        public List<GridCell> Cells => _inventory.Cells;

        public PlaceTestResult CanPlace(GridCell targetCell, Item item, bool ignoreItself)
        {
            return _inventory.CanPlace(targetCell, item, ignoreItself);
        }

        public bool TryRemove(Item item, out GridCell gridCell)
        {
            return _inventory.TryRemove(item, out gridCell);
        }

        public bool TryAdd(Item item)
        {
            return _inventory.TryAdd(item);
        }

        public int GridIndex(GridCell cell)
        {
            return _inventory.GridIndex(cell);
        }

        public int GridIndex(int x, int y)
        {
            return _inventory.GridIndex(x, y);
        }

        public bool TryAdd(GridCell cell, Item item)
        {
            return _inventory.TryAdd(cell, item);
        }

        public Item GetById(string itemId)
        {
            return _inventory.GetById(itemId);
        }
    }
}