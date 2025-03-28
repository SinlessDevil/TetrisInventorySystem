using System.Collections.Generic;
using System.Linq;
using Code.Inventory.Items.Data;
using UnityEngine;

namespace Code.Inventory.Items.Provider
{
    public class ItemDataProvider : IItemDataProvider
    {
        private const string ItemDataPath = "StaticData/Items";
        
        private Dictionary<string, ItemData> _itemsData;

        public void LoadData()
        {
            _itemsData = Resources
                .LoadAll<ItemData>(ItemDataPath)
                .ToDictionary(x => x.Id, x => x);
        }

        public IEnumerable<ItemData> AllItems => _itemsData.Values;
        public ItemData ForItemId(string itemId) => _itemsData.ContainsKey(itemId) ? _itemsData[itemId] : null;
    }   
}
