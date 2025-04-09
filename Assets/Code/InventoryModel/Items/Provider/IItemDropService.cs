using System.Collections.Generic;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using UI.Inventory;

namespace Code.InventoryModel.Items.Provider
{
    public interface IItemDropService
    {
        void Initialize(
            InventoryContainer inventoryContainer, 
            IItemPositionFinding itemPositionFinding);

        void Dispose();
        List<ItemContainer> DropItemContainers();
    }
}