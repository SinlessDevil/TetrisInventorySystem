using System.Collections.Generic;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using UI.Inventory;

namespace Code.InventoryModel.Items.Provider
{
    public interface IItemDropService
    {
        public void Initialize(InventoryContainer inventoryContainer, IItemPositionFinding itemPositionFinding);
        public void Dispose();
        public List<ItemContainer> DropItemContainers();
    }
}