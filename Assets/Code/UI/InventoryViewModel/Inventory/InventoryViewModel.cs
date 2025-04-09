using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UI.Inventory;
using Code.InventoryModel;
using Code.InventoryModel.Items.Provider;
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
        private readonly IItemDropService _itemDropService;
        private readonly IItemDataProvider _itemDataProvider;
        
        private List<SlotContainer> _slotContainers;
        private List<ItemContainer> _itemContainers;

        public InventoryViewModel(
            IInventory inventory,
            IItemPositionFinding itemPositionFinding, 
            IItemDropService itemDropService, 
            IItemDataProvider itemDataProvider) 
        {
            _inventory = inventory;
            _itemPositionFinding = itemPositionFinding;
            _itemDropService = itemDropService;
            _itemDataProvider = itemDataProvider;
        }

        public event Action<bool> EffectTogglePlayingDestroyGlowEvent;
        public event Action<bool> EffectTogglePlayingFreeAreaGlowEvent;

        public void InitializeViewModel(
            List<SlotContainer> slotContainers, 
            List<ItemContainer> itemContainers)
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
                SubscribeItemViewModel(x.ViewModel);
            });
        }
        
        public void Unsubscribe()
        {
            _itemContainers.ForEach(x =>
            {
                UnsubscribeItemViewModel(x.ViewModel);
            });
        }

        public void DropItems()
        {
            foreach (var itemContainer in _itemContainers)
            {
                var canPlace = _itemPositionFinding.TryToPlaceItemFreeAreaContainer(itemContainer.View.transform.position);
                if(!canPlace)
                    continue;
                
                CleanUpItemAsync(false, itemContainer).Forget();
            }
            
            List<Vector2> positions = _itemPositionFinding.GetRandomPositionsInFreeAreaContainer(_itemDataProvider.ItemDropData.CountItems);
            List<ItemContainer> itemContainers = _itemDropService.DropItemContainers();

            for (int i = 0; i < itemContainers.Count; i++)
            {
                itemContainers[i].ViewModel.SetPosition(positions[i]);
                itemContainers[i].View.Initialize(itemContainers[i].ViewModel);
                SubscribeItemViewModel(itemContainers[i].ViewModel);
                itemContainers[i].ViewModel.PlayEffectDropItem();
                _itemContainers.Add(itemContainers[i]);
            }
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

            //Try stack items out inventory 
            if (TryStackItemOutInventory(currentPosition, itemVM))
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

            //Try stack item in inventory
            if (TryStackItemInInventory(targetGridCell, itemVM))
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
        
        private void OnHandleOutlineGlowingItems(IItemViewModel itemVM)
        {
            List<ItemContainer> itemContainer = GetItemContainesByVM(itemVM);
            itemContainer.ForEach(x => x.ViewModel.PlayEffectOutlineGlow());
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
                
                 isCanMarge = gridCell.Item != null && 
                              item.Id == gridCell.Item.Id && 
                              item.InstanceId != gridCell.Item.InstanceId;

                targetGridCellIsCanMarge = gridCell;

                if (isCanMarge)
                    break;
            }

            UpdateColorSlotsToDefault();

            if (isCanMarge)
            {
                foreach (var slotData in _slotContainers)
                {
                    if (slotData.ViewModel.GridCell.Item == null)
                        continue;
            
                    if (targetGridCellIsCanMarge.Item.InstanceId == slotData.ViewModel.GridCell.Item.InstanceId)
                        slotData.ViewModel.SetColorReaction(true);
                }
                return;
            }

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
            CleanUpItemAsync(true, itemContainer).Forget();
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

        private bool TryStackItemOutInventory(Vector2 currentPosition, IItemViewModel itemVM)
        {
            bool isCanPlace = _itemPositionFinding.TryToPlaceItemFreeAreaContainer(currentPosition);
            if (!isCanPlace)
                return false;
            
            ItemContainer closesData = null;
            float minDistance = float.MaxValue;
            float maxDistance = 10000f;
            
            ItemContainer targetItemContainer = GetItemContainerByItem(itemVM.Item);
            List<ItemContainer> itemContainers = GetItemContainesByVM(itemVM);
            Vector2 targetPosition = targetItemContainer.View.transform.position;
            
            if (itemContainers.Contains(targetItemContainer))
                itemContainers.Remove(targetItemContainer);
            
            foreach (var slotData in itemContainers)
            {
                float distance = (targetPosition - (Vector2)slotData.View.transform.position).sqrMagnitude;
                if (distance > minDistance)
                    continue;

                minDistance = distance;
                closesData = slotData;
            }

            if (closesData == null)
                return false;
            
            float currentDistanceSlot = (targetPosition - (Vector2)closesData.View.transform.position).sqrMagnitude;
            Debug.Log(currentDistanceSlot);
            if (currentDistanceSlot > maxDistance)
                return false;
            
            int count = itemVM.Item.ItemCount;
            _inventory.TryRemove(itemVM.Item, out _);
            CleanUpItemAsync(false, targetItemContainer).Forget();
            
            closesData.ViewModel.Item.ItemCount += count;
            closesData.ViewModel.PlayEffectStackItem();
            
            return true;
        }
        
        private bool TryStackItemInInventory(GridCell targetGridCell, IItemViewModel itemVM)
        {
            PlaceTestResult placeTestResult = _inventory.CanPlace(targetGridCell, itemVM.Item, true);
            int gridIndex = _inventory.GridIndex(targetGridCell);
            List<int> indexShifts = itemVM.Item.InventoryPlacement.GetIndexShifts(InventorySize.Rows);
            for (int i = 0; i < indexShifts.Count; i++)
            {
                int targetIndex = gridIndex + indexShifts[i];
                if (targetIndex < 0 || targetIndex >= _inventory.Cells.Count)
                    continue;

                bool isPassed = !placeTestResult.Passed.Contains(targetIndex);
                bool isBlocked = !placeTestResult.Blocked.Contains(targetIndex);
                if (isPassed && isBlocked)
                    continue;

                GridCell gridCell = _inventory.Cells[targetIndex];
                InventoryModel.Items.Data.Item item = itemVM.Item;
                
                if (targetGridCell.Item == null)
                    return false;

                if (item.Id != targetGridCell.Item.Id)
                    return false;

                if(item == targetGridCell.Item)
                    return false;
                
                ItemContainer itemContainer = GetItemContainerByVM(itemVM);
                if(itemContainer == null)
                    return false;
                
                int count = item.ItemCount;
                _inventory.TryRemove(itemVM.Item, out _);
                CleanUpItemAsync(false, itemContainer).Forget();
                
                targetGridCell.Item.ItemCount += count;
                ItemContainer itemContainerByItem = GetItemContainerByItem(gridCell.Item);
                itemContainerByItem.ViewModel.PlayEffectStackItem();
                
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
            
            List<ItemContainer> itemContainer = GetItemContainesByVM(itemVM);
            itemContainer.ForEach(x => x.ViewModel.StopEffectOutlineGlow());
        }
        
        private void UpdateColorSlotsToDefault()
        {
            _slotContainers.ForEach(x=> x.ViewModel.SetToDefaultColorReaction());
        }

        private async UniTask CleanUpItemAsync(bool isWaited, ItemContainer itemContainer)
        {
            itemContainer.ViewModel.SetPosition(itemContainer.View.transform.localPosition);

            if (isWaited)
                await Task.Delay(400);
            else
                await Task.Yield(); 
            
            itemContainer.View.Dispose();
            itemContainer.View = null;
            itemContainer.ViewModel = null;
            _itemContainers.Remove(itemContainer);
            
            EffectTogglePlayingDestroyGlowEvent?.Invoke(false);
        }
        
        private void SubscribeItemViewModel(IItemViewModel itemVM)
        {
            itemVM.StartedDragViewEvent += OnHandleOutlineGlowingItems;
            itemVM.EndedDragViewEvent += OnHandlePlaceItem;
            itemVM.ChangedPositionViewEvent += OnUpdateColorToPlaceItem;
            itemVM.ChangedPositionViewEvent += OnUpdateDestroyGlowEffect;
            itemVM.ChangedPositionViewEvent += OnUpdateFreeAreaGlowEffect;
            itemVM.EffectDropItemEvent += OnHandlePlayEffectFilledSlot;
        }
        
        private void UnsubscribeItemViewModel(IItemViewModel itemVM)
        {
            itemVM.StartedDragViewEvent -= OnHandleOutlineGlowingItems;
            itemVM.EndedDragViewEvent -= OnHandlePlaceItem;
            itemVM.ChangedPositionViewEvent -= OnUpdateColorToPlaceItem;
            itemVM.ChangedPositionViewEvent -= OnUpdateDestroyGlowEffect;
            itemVM.ChangedPositionViewEvent -= OnUpdateFreeAreaGlowEffect;
            itemVM.EffectDropItemEvent -= OnHandlePlayEffectFilledSlot;
        }
        
        #region Getters

        private List<ItemContainer> GetItemContainesByVM(IItemViewModel itemVM) => _itemContainers
            .Where(x => x.ViewModel.Item.Id == itemVM.Item.Id)
            .ToList();

        private GridCell GetGridCellByRootPosition(int rootPositionX, int rootPositionY) => _slotContainers
            .Select(slotData => slotData.ViewModel.GridCell)
            .FirstOrDefault(gridCell => gridCell.GridX == rootPositionX && gridCell.GridY == rootPositionY);

        private ISlotViewModel GetSlotVMByIndex(int index) => index >= 0 && index < _slotContainers.Count ? 
            _slotContainers[index].ViewModel : null;

        private ItemContainer GetItemContainerByVM(IItemViewModel itemVM) => _itemContainers
            .FirstOrDefault(itemContainer => itemContainer.ViewModel == itemVM);

        private ItemContainer GetItemContainerByItem(InventoryModel.Items.Data.Item item) => _itemContainers
            .FirstOrDefault(itemContainer => itemContainer.ViewModel.Item.InstanceId == item.InstanceId);

        private List<SlotContainer> GetSlotDataByItem(InventoryModel.Items.Data.Item item) => _slotContainers
                .Where(slotData => slotData.ViewModel.Item != null)
                .Where(slotData => slotData.ViewModel.Item.InstanceId == item.InstanceId)
                .ToList();

        #endregion
    }
}