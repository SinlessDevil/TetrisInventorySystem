using System;
using Code.InventoryModel;

namespace Code.UI.InventoryViewModel.Slot
{
    public interface ISlotViewModel
    {
        public event Action<bool> ColoredFillSlotEvent;
        public event Action<bool> ColoredReactionSlotEvent;
        
        public GridCell GridCell { get; }
        public bool IsInteractableButton();
        public bool HasNecessaryLevel();
        public bool IsLockedSlotAndIsAvailableToBuy();
        public bool IsUnlockedSlot();
        public bool IsLockedSlot();
        
        public string GetTextLevel();
        public bool GetColorLockedSlot();

        void SetColorReaction(bool isCanPlace);
        void SetToDefaultColorReaction();
        
        void Subscribe();
        void Unsubscribe();
        event Action ChangedStateSlotEvent;
    }
}