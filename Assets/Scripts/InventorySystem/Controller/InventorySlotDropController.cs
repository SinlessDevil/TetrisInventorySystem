using System;
using UnityEngine;
using UnityEngine.EventSystems;
using InventorySystem.Abstract;

namespace InventorySystem.Controller
{
    public class InventorySlotDropController : MonoBehaviour, IDropHandler
    {
        public event Action<IInventorySlot> OnRefreshSlotEvent;

        public IInventorySlot slot { get; private set; }

        private InventoryController _inventoryController;

        private void Awake()
        {
            _inventoryController = GetComponentInParent<InventoryController>();
        }

        public void SetSlot(IInventorySlot newSlot) => slot = newSlot;
        public void OnDrop(PointerEventData eventData)
        {
            var otherItemController = eventData.pointerDrag.GetComponent<InventoryItemDragController>();
            var otherSlotController = otherItemController.GetComponentInParent<InventorySlotDropController>();
            var otherSlot = otherSlotController.slot;
            var inventory = _inventoryController.Inventory;

            inventory.TransitFromSlotToSlot(this, otherSlot, slot);

            RefreshSlot();
            otherSlotController.RefreshSlot();
        }
        public void RefreshSlot()
        {
            OnRefreshSlotEvent?.Invoke(slot);
        }
    }
}