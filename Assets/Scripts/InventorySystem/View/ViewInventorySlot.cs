using UnityEngine;
using InventorySystem.Abstract;
using InventorySystem.Controller;
using Extensions;

namespace InventorySystem.View
{
    public class ViewInventorySlot : MonoBehaviour
    {
        [SerializeField] private ViewInventoryItem _viewInventoryItem;

        private InventorySlotDropController _inventorySlotDropController;

        private void Awake()
        {
            InitComponent();
            Asserts();
        }
        private void InitComponent()
        {
            _inventorySlotDropController = GetComponent<InventorySlotDropController>();
        }
        private void Asserts()
        {
            _viewInventoryItem.LogErrorIfComponentNull();
            _inventorySlotDropController.LogErrorIfComponentNull();
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