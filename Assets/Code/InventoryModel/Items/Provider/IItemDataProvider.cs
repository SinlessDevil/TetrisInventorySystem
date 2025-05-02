using System.Collections.Generic;
using Code.InventoryModel.Items.Data;

namespace Code.InventoryModel.Items.Provider
{
    public interface IItemDataProvider
    {
        public IEnumerable<ItemData> AllItems { get; }
        public ItemData ForItemId(string itemId);
        public ItemDropData ItemDropData { get; }
        
        public void LoadData();
    }
}