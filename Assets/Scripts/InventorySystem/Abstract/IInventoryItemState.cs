namespace InventorySystem.Abstract
{
    public interface IInventoryItemState
    {
        int Amount { get; set; }
        bool IsEquipped { get; set; }
    }
}