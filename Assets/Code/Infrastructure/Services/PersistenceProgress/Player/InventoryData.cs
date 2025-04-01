using System;
using Code.Infrastructure.Services.PersistenceProgress.Player;
using Code.Inventory;
using Code.InventoryModel;

namespace Services.PersistenceProgress.Player
{
    [Serializable]
    public class InventoryData
    {
        public TetrisInventoryData PlayerInventory;
        public InventoryOpeningData InventoryOpening = new InventoryOpeningData();

        public InventoryData(TetrisInventoryData playerInventory)
        {
            PlayerInventory = playerInventory;
        }
    }
}