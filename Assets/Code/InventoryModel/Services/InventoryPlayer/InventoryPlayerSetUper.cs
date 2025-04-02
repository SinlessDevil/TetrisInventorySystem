using System;

namespace Code.InventoryModel.Services.InventoryPlayer
{
    public class InventoryPlayerSetUper : IInventoryPlayerSetUper
    {
        public IInventory Inventory { get; private set; }
        
        public void SetInventory(IInventory inventory)
        {
            Inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }
    }
}