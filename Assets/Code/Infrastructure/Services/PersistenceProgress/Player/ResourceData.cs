using System;

namespace Code.Infrastructure.Services.PersistenceProgress.Player
{
    public class ResourceData
    {
        public event Action InventoryPointsChangeEvent;
        public event Action InventroyLevelChangeEvent;
        
        public int InventoryPoints = 0;
        public int InventoryLevel = 1;
            
        public void SetInventoryPoints(int inventoryPoints)
        {
            InventoryPoints = inventoryPoints;
            InventoryPointsChangeEvent?.Invoke();
        }

        public void SetInventoryLevel(int inventoryLevel)
        {
            InventoryLevel = inventoryLevel;
            InventroyLevelChangeEvent?.Invoke();
        }
    }
}