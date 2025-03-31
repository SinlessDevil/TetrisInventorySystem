using System;
using Code.Inventory;
using JetBrains.Annotations;

namespace Services.PersistenceProgress.Player
{
    [Serializable]
    public class PlayerData
    {
        public InventoryData InventoryData;

        [UsedImplicitly]
        public PlayerData()
        {
            
        }
        
        public PlayerData(int columns, int rows)
        {
            InventoryData = new InventoryData(new TetrisInventoryData(columns, rows));
        }
    }
}