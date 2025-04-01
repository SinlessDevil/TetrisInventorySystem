using System;

namespace Code.Infrastructure.Services.PersistenceProgress.Player
{
    public class ResourceData
    {
        public event Action InventoryPointsChangeEvent;
            
        public int InventoryPoints = 0;
            
        public void SetInventoryPoints(int inventoryPoints)
        {
            InventoryPoints = inventoryPoints;
            InventoryPointsChangeEvent?.Invoke();
        }

    }
}