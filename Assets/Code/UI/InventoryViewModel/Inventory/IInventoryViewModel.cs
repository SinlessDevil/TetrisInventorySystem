using System;
using System.Collections.Generic;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;

namespace Code.UI.InventoryViewModel.Inventory
{
    public interface IInventoryViewModel
    {
        public event Action<bool> EffectTogglePlayingDestroyGlowEvent;
        
        public void Subscribe();
        public void Unsubscribe();
        void InitializeViewModel(List<SlotContainer> slotContainers, List<ItemContainer> itemContainers);
        void DisposeViewModel();
    }
}