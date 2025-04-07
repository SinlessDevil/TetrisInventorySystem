using Code.InventoryModel;

namespace Code.UI.InventoryViewModel.Slot
{
    public interface ISlotViewModel
    {
        GridCell GridCell { get; }
        bool IsInteractableButton();
        string GetTextLevel();
        bool HasNecessaryLevel();
        bool IsLockedSlotAndIsAvailableToBuy();
        bool IsUnlockedSlot();
        bool IsLockedSlot();
    }
}