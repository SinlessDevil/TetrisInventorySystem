using Code.Inventory.Items.Data;

namespace Code.Inventory.Items.Factory
{
    public interface IItemFactory
    {
        Item Create(string itemId);
    }
}