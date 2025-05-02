using Code.InventoryModel.Data;
using Services.PersistenceProgress.Player;

namespace Services.Factories.Inventory
{
    public interface IInventorySaveInitializer
    {
        InventoryData InventoryData { get; }
        void Initialize(InventoryBalance.ItemId[] initialItems, InventoryBorders defaultBorders);
        void OpenDefaultCells(InventoryBorders defaultBorders);
        void CleanupInventory();
        void PlaceInitialItems(InventoryBalance.ItemId[] initialItems);
    }
}