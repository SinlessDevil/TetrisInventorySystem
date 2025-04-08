using System;
using System.Collections.Generic;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;

namespace Code.UI.InventoryViewModel.Inventory
{
    public interface IInventoryViewModel
    {
        public event Action<bool> EffectTogglePlayingDestroyGlowEvent;
        public event Action<bool> EffectTogglePlayingFreeAreaGlowEvent;
        
        public void InitializeViewModel(List<SlotContainer> slotContainers, List<ItemContainer> itemContainers);
        public void DisposeViewModel();
        
        public void Subscribe();
        public void Unsubscribe();
    }
}