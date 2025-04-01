using System.Collections.Generic;
using Code.InventoryModel.Data;

namespace Code.InventoryModel.Services.InventoryDataProvider
{
    public interface IInventoryDataProvider
    {
        IEnumerable<InventoryExpandConfig> AllInventoryExpand { get; }
        void LoadData();
        InventoryExpandConfig InventoryExpandForLevel(int currentLevel);
        bool TryGetPointsForLevel(int currentLevel, out InventoryPointsConfig inventoryPointsConfig);
    }
}