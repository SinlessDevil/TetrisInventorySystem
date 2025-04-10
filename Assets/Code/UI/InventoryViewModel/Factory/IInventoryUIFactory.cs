using Code.UI.InventoryViewModel.Inventory;
using Code.UI.InventoryViewModel.Inventory.Chest;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Slot;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Factory
{
    public interface IInventoryUIFactory
    {
        public InventoryView CreateInventoryView();
        public SlotView CreateSlotView(RectTransform container);
        public ItemView CreateItemView(RectTransform container);
        public ChestView CreatChestView(RectTransform container);
    }
}