using System;
using System.Collections.Generic;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using Code.UI.InventoryViewModel.Slot;

namespace Code.UI.InventoryViewModel.Inventory
{
    public interface IInventoryViewModel
    {
        public event Action<bool> EffectTogglePlayingDestroyGlowEvent;
        public event Action<bool> EffectTogglePlayingFreeAreaGlowEvent;
        
        public void InitializeViewModel(List<SlotContainer> slotContainers, List<ItemContainer> itemContainers);
        public void DisposeViewModel();
        
        List<ItemView> GetItemViews();
        List<SlotContainer> GetSlotContainers();
        
        public void Subscribe();
        public void Unsubscribe();
    }
}