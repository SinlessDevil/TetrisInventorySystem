using System.Collections.Generic;
using System.Linq;
using Code.InventoryModel;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using UI.Inventory;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class InventoryViewModel : IInventoryViewModel
    {
        private readonly IInventory _inventory;
        private readonly IItemPositionFinding _itemPositionFinding;
        private readonly List<SlotContainer> _slotContainers;
        
        private List<ItemContainer> _itemContainers;

        public InventoryViewModel(
            IInventory inventory,
            IItemPositionFinding itemPositionFinding,
            List<SlotContainer> slotContainers,
            List<ItemContainer> itemContainer) 
        {
            _inventory = inventory;
            _itemPositionFinding = itemPositionFinding;
            _slotContainers = slotContainers;
            _itemContainers = itemContainer;
        }

        public void Subscribe()
        {
            _itemContainers.ForEach(x =>
            {
                x.ViewModel.EndedDragViewEvent += OnHandlePlaceItem;
            });
        }

        public void Unsubscribe()
        {
            _itemContainers.ForEach(x =>
            {
                x.ViewModel.EndedDragViewEvent -= OnHandlePlaceItem;
            });
        }

        private void OnHandlePlaceItem(Vector2 currentPosition, IItemViewModel itemVM)
        {
            GridCell targetGridCell = _itemPositionFinding.GetNeighbourGritCellByPosition(currentPosition);

            Debug.Log(targetGridCell);
            
            //check if item in out of grid
            if (targetGridCell == null)
            {
                itemVM.PlayAnimationReturnToTargetPosition();
                return;
            }
            
            //Try changed position item in slots
            if (TryChangedPositionItemInSlots(targetGridCell, itemVM))
            {
                itemVM.PlayAnimationReturnToTargetPosition();
                return;
            }
            
            //Just return item to target position
            itemVM.PlayAnimationReturnToTargetPosition();
        }
        
        private bool TryChangedPositionItemInSlots(GridCell targetGridCell, IItemViewModel itemVM)
        {
            InventoryModel.Items.Data.Item item = itemVM.Item;

            if (_inventory.CanPlace(targetGridCell, item, true) == false)
                return false;

            var oldGridCell = GetGridCellByRootPosition(item.InventoryPlacement.RootPositionX,
                item.InventoryPlacement.RootPositionY);

            if (oldGridCell != null)
                _inventory.TryRemove(item, out oldGridCell);

            _inventory.TryAdd(targetGridCell, item);
            return true;
        }
        
        private GridCell GetGridCellByRootPosition(int rootPositionX, int rootPositionY)
        {
            return _slotContainers.Select(slotData => slotData.ViewModel.GridCell)
                .FirstOrDefault(gridCell => gridCell.GridX == rootPositionX && gridCell.GridY == rootPositionY);
        }
    }
}