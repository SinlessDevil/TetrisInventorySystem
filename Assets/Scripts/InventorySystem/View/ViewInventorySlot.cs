using UnityEngine;
using InventorySystem.Abstract;
using InventorySystem.Controller;

namespace InventorySystem.View
{
    public class ViewInventorySlot : MonoBehaviour
    {
        [SerializeField] private ViewInventoryItem _viewInventoryItem;

        private InventorySlotDropController _inventorySlotDropController;

        private void Awake()
        {
            _inventorySlotDropController = GetComponent<InventorySlotDropController>();
        }

        private void OnEnable()
        {
            _inventorySlotDropController.OnRefreshSlotEvent += Refresh;
        }
        private void OnDisable()
        {
            _inventorySlotDropController.OnRefreshSlotEvent -= Refresh;
        }

        private void Refresh(IInventorySlot slot)
        {
            if(slot != null)
                _viewInventoryItem.Refresh(slot);
        }
    }
}