using Code.UI.InventoryViewModel.Inventory;

namespace Code.UI.InventoryViewModel.Factory
{
    public interface IInventoryUIFactory
    {
        InventoryView CreateInventoryView();
    }
}