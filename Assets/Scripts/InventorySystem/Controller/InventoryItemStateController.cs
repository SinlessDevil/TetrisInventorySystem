using System;
using InventorySystem.Abstract;

namespace InventorySystem.Controller
{
    [Serializable]
    public class InventoryItemStateController : IInventoryItemState
    {
        public int itemAmount;
        public bool isItemEquipped;

        public int Amount { get => itemAmount; set => itemAmount = value; }
        public bool IsEquipped { get => isItemEquipped; set => isItemEquipped = value; }

        public InventoryItemStateController()
        {
            itemAmount = 0;
            isItemEquipped = false;
        }
    }
}