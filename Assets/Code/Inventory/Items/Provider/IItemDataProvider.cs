using System.Collections.Generic;
using Code.Inventory.Items.Data;

namespace Code.Inventory.Items.Provider
{
    public interface IItemDataProvider
    {
        public IEnumerable<ItemData> AllItems { get; }
        public ItemData ForItemId(string itemId);
        void LoadData();
    }
}