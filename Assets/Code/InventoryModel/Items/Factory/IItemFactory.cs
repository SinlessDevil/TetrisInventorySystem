using Code.InventoryModel.Items.Data;

namespace Code.InventoryModel.Items.Factory
{
    public interface IItemFactory
    {
        Item Create(string itemId);
    }
}