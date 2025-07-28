using System;
using Code.InventoryModel;

namespace Code.Infrastructure.Services.PersistenceProgress.Player
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