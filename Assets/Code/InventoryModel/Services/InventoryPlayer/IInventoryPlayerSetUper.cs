namespace Code.InventoryModel.Services.InventoryPlayer
{
    public interface IInventoryPlayerSetUper
    {
        IInventory Inventory { get; }
        void SetInventory(IInventory inventory);
    }
}