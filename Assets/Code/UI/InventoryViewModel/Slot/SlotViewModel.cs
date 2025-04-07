using Code.Infrastructure.Services.PersistenceProgress;
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

        public GridCell GridCell => _gridCell;

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
                return !_inventoryExpandService.IsOpened(TargetIndexGridCell) &&
                       _inventoryExpandService.IsAvailableToBuy(TargetIndexGridCell);
            }

            return !_inventoryExpandService.IsOpened(TargetIndexGridCell) && 
                   _inventoryExpandService.IsEnoughPoints(TargetIndexGridCell);
        }
        
        public bool HasNecessaryLevel()
        {
            int level = _inventoryExpandService.GetLevelForAvailability(TargetIndexGridCell);
            return level != 99 && !IsInteractableButton();
        }
        
        public string GetTextLevel()
        {
            int level = _inventoryExpandService.GetLevelForAvailability(TargetIndexGridCell);
            
            if (level == 99 || IsInteractableButton())
                return "";  

            string textLevel = $"{level}\nLVL";
            return textLevel;
        }
        
        private int TargetIndexGridCell => _inventory.GridIndex(_gridCell);
    }
}