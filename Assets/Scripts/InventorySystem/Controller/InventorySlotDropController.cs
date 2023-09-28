using System;
using UnityEngine;
using UnityEngine.EventSystems;
using InventorySystem.Abstract;
using Extensions;

namespace InventorySystem.Controller
{
    public class InventorySlotDropController : MonoBehaviour, IDropHandler
    {
        public event Action<IInventorySlot> OnRefreshSlotEvent;

        public IInventorySlot Slot { get; private set; }

        private InventoryController _inventoryController;

        private void Awake()
        {
            InitComponent();
            Asserts();
        }
        private void InitComponent()
        {
            _inventoryController = GetComponentInParent<InventoryController>();
        }
        private void Asserts()
        {
            _inventoryController.LogErrorIfComponentNull();
        }

        public void SetSlot(IInventorySlot newSlot) => Slot = newSlot;
        public void OnDrop(PointerEventData eventData)
        {
            var otherItemController = eventData.pointerDrag.GetComponent<InventoryItemDragController>();
            var otherSlotController = otherItemController.GetComponentInParent<InventorySlotDropController>();
            var otherSlot = otherSlotController.Slot;
            var inventory = _inventoryController.Inventory;

            inventory.TransitFromSlotToSlot(this, otherSlot, Slot);

            RefreshSlot();
            otherSlotController.RefreshSlot();
        }
        public void RefreshSlot()
        {
            OnRefreshSlotEvent?.Invoke(Slot);
        }
    }
}