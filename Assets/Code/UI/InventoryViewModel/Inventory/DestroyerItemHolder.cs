namespace Code.UI.InventoryViewModel.Inventory
{
    public class DestroyerItemHolder : ContainerDropHolder
    {
        protected override void Subscribe()
        {
            _inventoryVm.EffectTogglePlayingDestroyGlowEvent += OnTogglePlayingGlowEffect;
        }
    
        protected override void Unsubscribe()
        {
            _inventoryVm.EffectTogglePlayingDestroyGlowEvent -= OnTogglePlayingGlowEffect;
        }
    }
}