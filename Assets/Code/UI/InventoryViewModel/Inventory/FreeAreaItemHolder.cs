namespace Code.UI.InventoryViewModel.Inventory
{
    public class FreeAreaItemHolder : ContainerDropHolder
    {
        protected override void Subscribe()
        {
            _inventoryVm.EffectTogglePlayingFreeAreaGlowEvent += OnTogglePlayingGlowEffect;
        }
    
        protected override void Unsubscribe()
        {
            _inventoryVm.EffectTogglePlayingFreeAreaGlowEvent -= OnTogglePlayingGlowEffect;
        }
    }
}