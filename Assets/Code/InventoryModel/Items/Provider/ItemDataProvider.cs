using System.Collections.Generic;
using System.Linq;
using Code.InventoryModel.Items.Data;
using UnityEngine;

namespace Code.InventoryModel.Items.Provider
{
    public class ItemDataProvider : IItemDataProvider
    {
        private const string ItemDataPath = "StaticData/Items";
        private const string ItemDropConfigPath = "StaticData/Inventory/ItemDropConfig";
        
        private Dictionary<string, ItemData> _itemsData;

        private ItemDropData _itemDropData;
        
        public void LoadData()
        {
            _itemsData = Resources
                .LoadAll<ItemData>(ItemDataPath)
                .ToDictionary(x => x.Id, x => x);

            _itemDropData = Resources
                .Load<ItemDropData>(ItemDropConfigPath);
        }

        public ItemDropData ItemDropData => _itemDropData;
        public IEnumerable<ItemData> AllItems => _itemsData.Values;
        public ItemData ForItemId(string itemId) => _itemsData.ContainsKey(itemId) ? _itemsData[itemId] : null;
    }   
}
