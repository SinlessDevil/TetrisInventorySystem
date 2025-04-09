using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UI.Inventory;
using Code.InventoryModel;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using Code.UI.InventoryViewModel.Slot;
using Cysharp.Threading.Tasks;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class InventoryViewModel : IInventoryViewModel
    {
        private readonly IInventory _inventory;
        private readonly IItemPositionFinding _itemPositionFinding;
        
        private List<SlotContainer> _slotContainers;
        private List<ItemContainer> _itemContainers;

        public InventoryViewModel(
            IInventory inventory,
            IItemPositionFinding itemPositionFinding) 
        {
            _inventory = inventory;
            _itemPositionFinding = itemPositionFinding;
        }

        public event Action<bool> EffectTogglePlayingDestroyGlowEvent;
        public event Action<bool> EffectTogglePlayingFreeAreaGlowEvent;

        public void InitializeViewModel(List<SlotContainer> slotContainers, List<ItemContainer> itemContainers)
        {
            _slotContainers = slotContainers;
            _itemContainers = itemContainers;
        }

        public void DisposeViewModel()
        {
            _slotContainers.ForEach(x => x.ViewModel.Unsubscribe());
            
            _itemContainers.ForEach(x=> x.View.Dispose());
            _slotContainers.ForEach(x=> x.View.Dispose());
            
            _itemContainers.Clear();
            _slotContainers.Clear();
        }
        
        public List<ItemView> GetItemViews() => _itemContainers.Select(x => x.View).ToList();

        public List<SlotContainer> GetSlotContainers() => _slotContainers;
        
        public void Subscribe()
        {
            _itemContainers.ForEach(x =>
            {
                x.ViewModel.EndedDragViewEvent += OnHandlePlaceItem;
                x.ViewModel.ChangedPositionViewEvent += OnUpdateColorToPlaceItem;
                x.ViewModel.ChangedPositionViewEvent += OnUpdateDestroyGlowEffect;
                x.ViewModel.ChangedPositionViewEvent += OnUpdateFreeAreaGlowEffect;
                x.ViewModel.EffectDropItemEvent += OnHandlePlayEffectFilledSlot;
            });
        }
        
        public void Unsubscribe()
        {
            _itemContainers.ForEach(x =>
            {
                x.ViewModel.EndedDragViewEvent -= OnHandlePlaceItem;
                x.ViewModel.ChangedPositionViewEvent -= OnUpdateColorToPlaceItem;
                x.ViewModel.ChangedPositionViewEvent -= OnUpdateDestroyGlowEffect;
                x.ViewModel.ChangedPositionViewEvent -= OnUpdateFreeAreaGlowEffect;
                x.ViewModel.EffectDropItemEvent -= OnHandlePlayEffectFilledSlot;
            });
        }

        private void OnHandlePlaceItem(Vector2 currentPosition, IItemViewModel itemVM)
        {
            GridCell targetGridCell = _itemPositionFinding.GetNeighbourGritCellByPosition(currentPosition);

            //Try destroy item in placemant container destroy holder 
            if (TryDestroyItem(currentPosition, itemVM))
            {
                UpdateViewInventory(itemVM);
                return;
            }
            
            //Try drop item out of inventory
            if (TryDropItemOutInventory(currentPosition, itemVM))
            {
                UpdateViewInventory(itemVM);
                return;
            }
            
            //check if item in out of grid
            if (targetGridCell == null)
            {
                UpdateViewInventory(itemVM);
                return;
            }
            
            //Try changed position item in slots
            if (TryChangedPositionItemInSlots(targetGridCell, itemVM))
            {
                UpdateViewInventory(itemVM);
                return;
            }
            
            //Just return item to target position
            UpdateViewInventory(itemVM);
        }
        
        private void OnUpdateColorToPlaceItem(Vector2 currentPosition, IItemViewModel itemVM)
        {
            var isCanPlace = _itemPositionFinding.TryToPlaceItemInInventory(currentPosition);
            if (isCanPlace == false)
            {
                UpdateColorSlotsToDefault();
                return;
            }

            GridCell targetGridCell = _itemPositionFinding.GetNeighbourGritCellByPosition(currentPosition);
            InventoryModel.Items.Data.Item item = itemVM.Item;

            if (targetGridCell == null)
                return;

            GridCell targetGridCellIsCanMarge = null;
            PlaceTestResult placeTestResult = _inventory.CanPlace(targetGridCell, item, true);

            bool isCanMarge = false;
            int gridIndex = _inventory.GridIndex(targetGridCell);
            List<int> indexShifts = itemVM.Item.InventoryPlacement.GetIndexShifts(InventorySize.Rows);
            
            for (int i = 0; i < indexShifts.Count; i++)
            {
                int targetIndex = gridIndex + indexShifts[i];
                if (targetIndex < 0 || targetIndex >= _inventory.Cells.Count)
                    continue;

                var isPassed = !placeTestResult.Passed.Contains(targetIndex);
                var isBlocked = !placeTestResult.Blocked.Contains(targetIndex);
                if (isPassed && isBlocked)
                    continue;

                GridCell gridCell = _inventory.Cells[targetIndex];

                //TODO: Added merge service and unlocked this code
                
                // isCanMarge = gridCell.Item != null
                //              && _mergeService.CanMerge(item.Id, gridCell.Item.Id, out _)
                //              && item.InstanceId != gridCell.Item.InstanceId;

                targetGridCellIsCanMarge = gridCell;

                // if (isCanMarge)
                //     break;
            }

            UpdateColorSlotsToDefault();

            // if (isCanMarge)
            // {
            //     foreach (var slotData in _slotContainers)
            //     {
            //         if (slotData.ViewModel.GridCell.Item == null)
            //             continue;
            //
            //         if (targetGridCellIsCanMarge.Item.InstanceId == slotData.ViewModel.GridCell.Item.InstanceId)
            //             slotData.ViewModel.SetColorReaction(true);
            //     }
            //     return;
            // }

            foreach (var blocked in placeTestResult.Blocked)
            {
                var slotPm = GetSlotVMByIndex(blocked);
                slotPm?.SetColorReaction(false);
            }

            foreach (var passed in placeTestResult.Passed)
            {
                var slotPm = GetSlotVMByIndex(passed);
                slotPm?.SetColorReaction(placeTestResult.Blocked.Count == 0);
            }
        }
        
        private void OnUpdateDestroyGlowEffect(Vector2 currentPosition, IItemViewModel itemVM)
        {
            bool isCanPlace = _itemPositionFinding.TryToPlaceItemInDestroyContainer(currentPosition);
            
            if (isCanPlace)
            {
                EffectTogglePlayingDestroyGlowEvent?.Invoke(true);
                return;
            }
            
            EffectTogglePlayingDestroyGlowEvent?.Invoke(false);
        }
        
        private void OnUpdateFreeAreaGlowEffect(Vector2 currentPosition, IItemViewModel itemVM)
        {
            bool isCanPlace = _itemPositionFinding.TryToPlaceItemFreeAreaContainer(currentPosition);
            
            if (isCanPlace)
            {
                EffectTogglePlayingFreeAreaGlowEvent?.Invoke(true);
                return;
            }
            
            EffectTogglePlayingFreeAreaGlowEvent?.Invoke(false);
        }
        
        private void OnHandlePlayEffectFilledSlot(IItemViewModel itemVM)
        {
            var slotContainers = GetSlotDataByItem(itemVM.Item);
            slotContainers.ForEach(x=> x.ViewModel.PlayEffectFilledSlot());
        }

        private bool TryDestroyItem(Vector2 currentPosition, IItemViewModel itemVM)
        {
            bool isCanPlace = _itemPositionFinding.TryToPlaceItemInDestroyContainer(currentPosition);
            if (!isCanPlace)
                return false;

            ItemContainer itemContainer = GetItemContainerByVM(itemVM);
            if(itemContainer == null)
                return false;

            _inventory.TryRemove(itemVM.Item, out _);
            CleanUpItemAsync(itemContainer).Forget();
            return true;
        }

        private bool TryDropItemOutInventory(Vector2 currentPosition, IItemViewModel itemVM)
        {
            bool isCanPlace = _itemPositionFinding.TryToPlaceItemFreeAreaContainer(currentPosition);
            if (!isCanPlace)
                return false;

            ItemContainer itemContainer = GetItemContainerByVM(itemVM);
            if(itemContainer == null)
                return false;
            
            if (_inventory.TryRemove(itemVM.Item, out _))
            {
                itemContainer.ViewModel.SetPosition(itemContainer.View.transform.localPosition);
                EffectTogglePlayingFreeAreaGlowEvent?.Invoke(false);
                return true;
            }
            
            return false;
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

        private void UpdateViewInventory(IItemViewModel itemVM)
        {
            itemVM.PlayAnimationReturnToTargetPosition();
            UpdateColorSlotsToDefault();
        }
        
        private void UpdateColorSlotsToDefault()
        {
            _slotContainers.ForEach(x=> x.ViewModel.SetToDefaultColorReaction());
        }

        private async UniTask CleanUpItemAsync(ItemContainer itemContainer)
        {
            itemContainer.ViewModel.SetPosition(itemContainer.View.transform.localPosition);
            
            await Task.Delay(400);
            
            itemContainer.View.Dispose();
            _itemContainers.Remove(itemContainer);
            
            EffectTogglePlayingDestroyGlowEvent?.Invoke(false);
        }
        
        #region Getters

        private GridCell GetGridCellByRootPosition(int rootPositionX, int rootPositionY)
        {
            return _slotContainers.Select(slotData => slotData.ViewModel.GridCell)
                .FirstOrDefault(gridCell => gridCell.GridX == rootPositionX && gridCell.GridY == rootPositionY);
        }
        
        private ISlotViewModel GetSlotVMByIndex(int index)
        {
            return index >= 0 && index < _slotContainers.Count ? _slotContainers[index].ViewModel : null;
        }
        
        private ItemContainer GetItemContainerByVM(IItemViewModel itemVM)
        {
            return _itemContainers.FirstOrDefault(itemContainer => itemContainer.ViewModel == itemVM);
        }
        
        private List<SlotContainer> GetSlotDataByItem(InventoryModel.Items.Data.Item item)
        {
            List<SlotContainer> slotDatas = new List<SlotContainer>();
            foreach (var slotData in _slotContainers)
            {
                if (slotData.ViewModel.Item == null)
                    continue;
                
                if (slotData.ViewModel.Item.InstanceId == item.InstanceId)
                    slotDatas.Add(slotData);
            }

            return slotDatas;
        }

        #endregion
    }
}