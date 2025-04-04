using Code.UI.InventoryViewModel.Inventory;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Slot;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Factory
{
    public interface IInventoryUIFactory
    {
        InventoryView CreateInventoryView();
        SlotView CreateSlotView(RectTransform container);
        ItemView CreateItemView(RectTransform container);
    }
}