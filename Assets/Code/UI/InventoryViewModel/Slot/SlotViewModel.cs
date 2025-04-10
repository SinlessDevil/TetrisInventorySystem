using System;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Inventory;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel;

namespace Code.UI.InventoryViewModel.Slot
{
    public class SlotViewModel : ISlotViewModel
    {
        private readonly GridCell _gridCell;
        private readonly IInventory _inventory;
        private readonly IInventoryExpandService _inventoryExpandService;
        private readonly IPersistenceProgressService _persistenceProgressService;

        public SlotViewModel(
            GridCell gridCell,
            IInventory inventory,
            IInventoryExpandService inventoryExpandService,
            IPersistenceProgressService persistenceProgressService)
        {
            _gridCell = gridCell;
            _inventory = inventory;
            _inventoryExpandService = inventoryExpandService;
            _persistenceProgressService = persistenceProgressService;
        }

        public event Action ChangedStateSlotEvent;
        public event Action TryUnlockedSlotEvent;
        
        public event Action<bool> ColoredFillSlotEvent;
        public event Action<bool> ColoredReactionSlotEvent;
        
        public event Action EffectFilledSlotEvent;

        public GridCell GridCell => _gridCell;
        public InventoryModel.Items.Data.Item Item => _gridCell.Item;

        public bool IsUnlockedSlot() => _inventoryExpandService.IsOpened(TargetIndexGridCell);

        public bool IsLockedSlot() => !_inventoryExpandService.IsOpened(TargetIndexGridCell);
        
        public bool IsInteractableButton() => _inventoryExpandService.IsAvailableToBuy(TargetIndexGridCell) && 
                                              _inventoryExpandService.IsEnoughPoints(TargetIndexGridCell) && 
                                              IsLockedSlot();

        public bool IsLockedSlotAndIsAvailableToBuy()
        {
            if (!_inventoryExpandService.IsOpened(TargetIndexGridCell) && 
                _inventoryExpandService.IsEnoughPoints(TargetIndexGridCell))
            {
                return _inventoryExpandService.IsAvailableToBuy(TargetIndexGridCell);
            }

            return !_inventoryExpandService.IsOpened(TargetIndexGridCell) && 
                   _inventoryExpandService.IsEnoughPoints(TargetIndexGridCell);
        }
        
        public bool HasNecessaryLevel()
        {
            int level = _inventoryExpandService.GetLevelForAvailability(TargetIndexGridCell);
            return level == 99 || IsInteractableButton();
        }
        
        public string GetTextLevel()
        {
            int level = _inventoryExpandService.GetLevelForAvailability(TargetIndexGridCell);
            
            if (level == 99 || IsInteractableButton())
                return "";  

            string textLevel = $"{level}\nLVL";
            return textLevel;
        }

        public bool GetColorLockedSlot()
        {
            return _gridCell.IsFree;
        }

        public void SetColorReaction(bool isCanPlace) =>
            ColoredReactionSlotEvent?.Invoke(isCanPlace);

        public void SetToDefaultColorReaction() =>  
            ColoredFillSlotEvent?.Invoke(_gridCell.Item == null);

        public void PlayEffectFilledSlot() =>
            EffectFilledSlotEvent?.Invoke();
        
        public void Subscribe()
        {
            _inventory.OnItemRemoved += OnRemovedItem;
            _inventory.OnItemAdded += OnAddedItem;
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent += OnUpdateStateSlots;
            _persistenceProgressService.PlayerData.ResourceData.InventroyLevelChangeEvent += OnUpdateStateSlots;
        }
        public void Unsubscribe()
        {
            _inventory.OnItemRemoved -= OnRemovedItem;
            _inventory.OnItemAdded -= OnAddedItem;
            _persistenceProgressService.PlayerData.ResourceData.InventoryPointsChangeEvent -= OnUpdateStateSlots;
            _persistenceProgressService.PlayerData.ResourceData.InventroyLevelChangeEvent -= OnUpdateStateSlots;
        }
        
        private void OnRemovedItem(InventoryActionData inventoryActionData)
        {
            if (_gridCell.Item == null)
                ColoredFillSlotEvent?.Invoke(true);
        }
        
        private void OnAddedItem(InventoryActionData inventoryActionData)
        {
            if (_gridCell.Item != null)
                ColoredFillSlotEvent?.Invoke(false);
        }
        
        private void OnUpdateStateSlots()
        {
            ChangedStateSlotEvent?.Invoke();
        }
        
        public void TryToUnlockSlot()
        {
            if(IsLockedSlot() == false)
                return;
            
            if (_inventoryExpandService.IsEnoughPoints(TargetIndexGridCell) == false)
                return;

            if (_inventoryExpandService.IsAvailableToBuy(TargetIndexGridCell))
                _inventoryExpandService.Open(TargetIndexGridCell);
            
            ChangedStateSlotEvent?.Invoke();
            TryUnlockedSlotEvent?.Invoke();
        }
        
        private int TargetIndexGridCell => _inventory.GridIndex(_gridCell);
    }
}