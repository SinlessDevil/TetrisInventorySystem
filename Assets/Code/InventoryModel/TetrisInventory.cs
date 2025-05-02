using System;
using System.Collections.Generic;
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

        public void WithCondition(IInventoryAddCondition condition)
        {
            _inventoryAddConditions.Add(condition ?? throw new ArgumentNullException(nameof(condition)));
        }

        public void WithoutCondition(IInventoryAddCondition condition)
        {
            _inventoryAddConditions.Remove(condition);
        }
        
        public PlaceTestResult CanPlace(GridCell targetCell, Item item, bool ignoreItself)
        {
            return CanPlace(item, GridIndex(targetCell), ignoreItself);
        }

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

        public bool TryAdd(GridCell cell, Item item)
        {
            int gridIndex = GridIndex(cell);

            if (CanPlace(item, gridIndex, false))
            {
                Place(item, gridIndex);
                return true;
            }
            
            return false;
        }

        public bool TryAdd(Item item)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                if (!CanPlace(item, i, false))
                    continue;

                Place(item, i);
                return true;
            }

            return false;
        }

        public Item GetById(string itemId)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Item item = Items[i];
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

        public int GridIndex(int x, int y)
        {
            return x * Rows + y;
        }

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

            var indexShifts = item.InventoryPlacement.GetIndexShifts(Rows);

            for (int i = 0; i < indexShifts.Count; i++)
            {
                int indexShift = indexShifts[i];
                int targetIndex = gridIndex + indexShift;

                Cells[targetIndex].Place(item);
            }
        }

        private void ClearCells(Item item)
        {
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].Item != item)
                    continue;

                Cells[i].Clear();
            }
        }

        private PlaceTestResult CanPlace(Item item, int gridIndex, bool ignoreItself)
        {
            List<int> passed = new List<int>();
            List<int> blocked = new List<int>();
            
            List<int> indexesShifts = item.InventoryPlacement.GetIndexShifts(Rows);
            List<GridCell> willPlaced = new List<GridCell>();
            List<GridCell> allTested = new List<GridCell>();

            for (int i = 0; i < indexesShifts.Count; i++)
            {
                int indexShift = indexesShifts[i];
                int targetIndex = gridIndex + indexShift;

                if (targetIndex < 0)
                {
                    blocked.Add(targetIndex);
                    continue;
                }

                if (targetIndex >= Cells.Count)
                {
                    blocked.Add(targetIndex);
                    continue;
                }

                GridCell targetCell = Cells[targetIndex];
                allTested.Add(targetCell);

                if (targetCell.IsOccupied)
                {
                    bool isMe = targetCell.Item.InstanceId == item.InstanceId;
                    
                    if(!(ignoreItself && isMe))
                    {
                        blocked.Add(targetIndex);
                        continue;
                    }
                }

                for (var index = 0; index < _inventoryAddConditions.Count; index++)
                {
                    IInventoryAddCondition inventoryAddCondition = _inventoryAddConditions[index];

                    if (!inventoryAddCondition.CanPlace(item, targetIndex))
                    {
                        blocked.Add(targetIndex);
                    }
                }
                willPlaced.Add(targetCell);
                passed.Add(targetIndex);
            }

            bool isValid = IsConsistent();

            for (int i = 0; i < _inventoryAddConditions.Count; i++)
            {
                IInventoryAddCondition inventoryAddCondition = _inventoryAddConditions[i];
                
                if (!inventoryAddCondition.IsValid(willPlaced))
                    isValid = false;
            }

            bool isSuccess = isValid && blocked.Count == 0;
            
            return new PlaceTestResult(isSuccess, passed, blocked);

            bool IsConsistent()
            {
                if (willPlaced.Count <= 1)
                    return true;

                bool allConsistent = true;
                for (int i = 0; i < allTested.Count; i++)
                {
                    GridCell placedCell = allTested[i];

                    bool isConsistent = false;
                    for (int j = 0; j < placedCell.Neighbors.Length; j++)
                    {
                        GridCell neighbor = placedCell.Neighbors[j];
                        if (neighbor == null)
                            continue;

                        if (!allTested.Contains(neighbor))
                            continue;

                        isConsistent = true;
                        break;
                    }

                    if (isConsistent)
                        continue;
                    
                    allConsistent = false;
                    
                    int index = GridIndex(placedCell);
                    blocked.Remove(index);
                    passed.Remove(index);
                }

                return allConsistent;
            }
        }

        private void NotifyOnItemAdded(Item item)
        {
            OnItemAdded?.Invoke(new InventoryActionData
            {
                ItemId = item.Id
            });
        }
        
        private void NotifyOnItemRemove(Item item)
        {
            OnItemRemoved?.Invoke(new InventoryActionData
            {
                ItemId = item.Id
            });
        }
    }
}