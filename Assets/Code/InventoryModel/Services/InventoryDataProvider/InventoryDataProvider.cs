using System.Collections.Generic;
using System.Linq;
using Code.InventoryModel.Data;
using UnityEngine;

namespace Code.InventoryModel.Services.InventoryDataProvider
{
    public class InventoryDataProvider : IInventoryDataProvider
    {
        private const string InventoryExpandDataPath = "StaticData/Inventory/InventoryExpandData";
        private const string InventoryPointsDataPath = "StaticData/Inventory/InventoryPointsData";
        
        private Dictionary<int, InventoryExpandConfig> _expandData;
        private Dictionary<int, InventoryPointsConfig> _inventoryPointsData;
        
        public IEnumerable<InventoryExpandConfig> AllInventoryExpand => _expandData.Values;

        public void LoadData()
        {
            _expandData = Resources
                .Load<InventoryExpandData>(InventoryExpandDataPath)
                .Configs
                .ToDictionary(x => x.Level, x => x);
            
            _inventoryPointsData = Resources
                .Load<InventoryPointsData>(InventoryPointsDataPath)
                .Configs
                .ToDictionary(x => x.Level, x => x);
        }
        
        public InventoryExpandConfig InventoryExpandForLevel(int currentLevel) =>
            _expandData[Mathf.Min(currentLevel, _expandData.Count - 1)];
        
        public bool TryGetPointsForLevel(int currentLevel, out InventoryPointsConfig inventoryPointsConfig) =>
            _inventoryPointsData.TryGetValue(currentLevel, out inventoryPointsConfig);
    }
}