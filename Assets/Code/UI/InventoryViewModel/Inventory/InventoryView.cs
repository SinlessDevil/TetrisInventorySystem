using UnityEngine;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private RectTransform _slotsContainer;
        [SerializeField] private RectTransform _itemsContainer;
        [Space(10)] [Header("Additional")]
        [SerializeField] private DestroyItemHolder _destroyItemHolder;

        private IInventoryViewModel _inventoryVM;

        private void OnValidate()
        {
            if (_destroyItemHolder == null)
                _destroyItemHolder.GetComponentInChildren<DestroyItemHolder>();
        }

        public void Initialize(IInventoryViewModel inventoryM)
        {
            _inventoryVM = inventoryM;
            
            _destroyItemHolder.Initialize(_inventoryVM);
        }

        public void Dispose()
        {
            _destroyItemHolder.Dispose();
            
            Destroy(this.gameObject);
        }
        
        public RectTransform SlotsContainer => _slotsContainer;
        public RectTransform ItemsContainer => _itemsContainer;
        public RectTransform DestroyItemContainer => _destroyItemHolder.DestroyItemContainer;
    }
}