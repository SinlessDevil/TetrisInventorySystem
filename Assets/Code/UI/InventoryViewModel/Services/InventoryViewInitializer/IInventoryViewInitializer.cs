namespace Code.UI.InventoryViewModel.Services.InventoryViewInitializer
{
    public interface IInventoryViewInitializer
    {
        void OpenInventory();
        void CloseInventory();
        bool HasOpenInventory { get; }
    }
}