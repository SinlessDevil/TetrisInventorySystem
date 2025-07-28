using System;
using System.Collections.Generic;
using System.Linq;
using Code.Inventory;
using Code.InventoryModel.InventoryAddCondition;
using Code.InventoryModel.Items.Data;

namespace Code.InventoryModel
{
    public class TetrisInventory : IInventory
    {
        private readonly TetrisInventoryData _data;
        private readonly List<IInventoryAddCondition> _inventoryAddConditions = new();

        public TetrisInventory(TetrisInventoryData data)
        {
            _data = data;
        }

        public event Action<InventoryActionData> OnItemAdded;
        public event Action<InventoryActionData> OnItemRemoved;

        public List<Item> Items => _data.Items;
        public List<GridCell> Cells => _data.Cells;

        private int Columns => _data.Columns;
        private int Rows => _data.Rows;

        public void WithCondition(IInventoryAddCondition condition) => 
            _inventoryAddConditions.Add(condition ?? throw new ArgumentNullException(nameof(condition)));

        public void WithoutCondition(IInventoryAddCondition condition) => 
            _inventoryAddConditions.Remove(condition);

        public PlaceTestResult CanPlace(GridCell targetCell, Item item, bool ignoreItself) => 
            CanPlace(item, GridIndex(targetCell), ignoreItself);

        public bool TryRemove(Item item, out GridCell gridCell)
        {
            bool isRemoved = Items.Remove(item);

            if (isRemoved)
            {
                ClearCells(item);
                NotifyOnItemRemove(item);

                InventoryPlacement placement = item.InventoryPlacement;
                int gridIndex = GridIndex(placement.RootPositionX, placement.RootPositionY);
                gridCell = Cells[gridIndex];

                return true;
            }

            gridCell = null;
            return false;
        }

        public bool TryAdd(GridCell cell, Item item) => 
            TryAddAt(GridIndex(cell), item);

        public bool TryAdd(Item item)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                if (TryAddAt(i, item))
                    return true;
            }

            return false;
        }

        private bool TryAddAt(int gridIndex, Item item)
        {
            if (!CanPlace(item, gridIndex, false))
                return false;

            Place(item, gridIndex);
            return true;
        }

        public Item GetById(string itemId)
        {
            foreach (Item item in Items)
            {
                if (item.Id == itemId)
                    return item;
            }

            throw new InvalidOperationException($"no item for id {itemId}");
        }

        public int GridIndex(GridCell cell)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].HasSamePosition(cell))
                    return i;
            }

            throw new InvalidOperationException($"no cell with same pos {cell.GridX}, {cell.GridY}");
        }

        public int GridIndex(int x, int y) => x * Rows + y;

        private void Place(Item item, int gridIndex)
        {
            Items.Add(item);
            FillCells(item, gridIndex);
            NotifyOnItemAdded(item);
        }

        private void FillCells(Item item, int gridIndex)
        {
            GridCell cell = Cells[gridIndex];
            item.InventoryPlacement.SetRootPosition(cell.GridX, cell.GridY);

            List<int> indexShifts = item.InventoryPlacement.GetIndexShifts(Rows);

            foreach (int index in indexShifts)
            {
                int targetIndex = gridIndex + index;
                Cells[targetIndex].Place(item);
            }
        }

        private void ClearCells(Item item)
        {
            foreach (GridCell cell in Cells)
            {
                if (cell.Item == item)
                {
                    cell.Clear();
                }
            }
        }

        private PlaceTestResult CanPlace(Item item, int gridIndex, bool ignoreItself)
        {
            List<int> passed = new();
            List<int> blocked = new();
            List<int> indexShifts = item.InventoryPlacement.GetIndexShifts(Rows);
            List<GridCell> willPlaced = new();
            List<GridCell> allTested = new();

            foreach (int t in indexShifts)
            {
                int targetIndex = gridIndex + t;

                if (targetIndex < 0 || targetIndex >= Cells.Count)
                {
                    blocked.Add(targetIndex);
                    continue;
                }

                GridCell targetCell = Cells[targetIndex];
                allTested.Add(targetCell);

                if (targetCell.IsOccupied)
                {
                    bool isMe = targetCell.Item.InstanceId == item.InstanceId;
                    if (!(ignoreItself && isMe))
                    {
                        blocked.Add(targetIndex);
                        continue;
                    }
                }

                blocked.AddRange(from t1 in _inventoryAddConditions where !t1
                        .CanPlace(item, targetIndex) select targetIndex);

                willPlaced.Add(targetCell);
                passed.Add(targetIndex);
            }

            bool isValid = IsConsistent(allTested, willPlaced, blocked, passed);

            foreach (IInventoryAddCondition inventoryAddCondition in _inventoryAddConditions)
            {
                if (!inventoryAddCondition.IsValid(willPlaced))
                    isValid = false;
            }

            bool isSuccess = isValid && blocked.Count == 0;
            return new PlaceTestResult(isSuccess, passed, blocked);
        }

        private bool IsConsistent(List<GridCell> allTested, List<GridCell> willPlaced, List<int> blocked, List<int> passed)
        {
            if (willPlaced.Count <= 1)
                return true;

            bool allConsistent = true;

            foreach (GridCell placedCell in allTested)
            {
                bool isConsistent = placedCell.Neighbors
                    .Any(neighbor => neighbor != null && allTested
                        .Contains(neighbor));

                if (isConsistent)
                    continue;

                allConsistent = false;
                int index = GridIndex(placedCell);
                blocked.Remove(index);
                passed.Remove(index);
            }

            return allConsistent;
        }

        private void NotifyOnItemAdded(Item item) =>
            OnItemAdded?.Invoke(new InventoryActionData { ItemId = item.Id });

        private void NotifyOnItemRemove(Item item) =>
            OnItemRemoved?.Invoke(new InventoryActionData { ItemId = item.Id });
    }
}