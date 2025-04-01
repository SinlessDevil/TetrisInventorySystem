using Code.InventoryModel.Data;

namespace Code.Inventory.Services.InventoryExpand
{
    public interface IInventoryExpandService
    {
        bool IsOpened(int targetIndex);
        bool IsAvailableToBuy(int targetIndex);
        bool IsEnoughPoints(int targetIndex);
        void Open(int targetIndex);
        int GetLevelForAvailability(int targetIndex);
        InventoryBorders DefaultOpened { get; }
        void SetDefaultOpenedBorders(InventoryBorders defaultBorder);
    }
}