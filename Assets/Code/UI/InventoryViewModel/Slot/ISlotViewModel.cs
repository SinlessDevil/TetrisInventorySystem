using System;
using Code.InventoryModel;

namespace Code.UI.InventoryViewModel.Slot
{
    public interface ISlotViewModel
    {
        public event Action ChangedStateSlotEvent;
        
        public event Action<bool> ColoredFillSlotEvent;
        public event Action<bool> ColoredReactionSlotEvent;
        
        public event Action EffectFilledSlotEvent;
        
        public GridCell GridCell { get; }
        public InventoryModel.Items.Data.Item Item { get; }
        public bool IsInteractableButton();
        public bool HasNecessaryLevel();
        public bool IsLockedSlotAndIsAvailableToBuy();
        public bool IsUnlockedSlot();
        public bool IsLockedSlot();
        
        public string GetTextLevel();
        public bool GetColorLockedSlot();

        public void SetColorReaction(bool isCanPlace);
        public void SetToDefaultColorReaction();
        
        public void PlayEffectFilledSlot();
        
        public void Subscribe();
        public void Unsubscribe();
    }
}