using UnityEngine;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private RectTransform _slotsContainer;
        [SerializeField] private RectTransform _itemsContainer;
        [SerializeField] private RectTransform _itemDragContainer;
        [Space(10)] [Header("Additional")]
        [SerializeField] private DestroyerItemHolder _destroyItemHolder;
        [SerializeField] private FreeAreaItemHolder _freeAreaItemHolder;

        private IInventoryViewModel _inventoryVM;

        private void OnValidate()
        {
            if (_destroyItemHolder == null)
                _destroyItemHolder.GetComponentInChildren<DestroyerItemHolder>();
            
            if (_freeAreaItemHolder == null)
                _freeAreaItemHolder.GetComponentInChildren<FreeAreaItemHolder>();
        }

        public void Initialize(IInventoryViewModel inventoryM)
        {
            _inventoryVM = inventoryM;
            
            _destroyItemHolder.Initialize(_inventoryVM);
            _freeAreaItemHolder.Initialize(_inventoryVM);
        }

        public void Dispose()
        {
            _destroyItemHolder.Dispose();
            _freeAreaItemHolder.Dispose();
            
            Destroy(this.gameObject);
        }
        
        public RectTransform SlotsContainer => _slotsContainer;
        public RectTransform ItemsContainer => _itemsContainer;
        public RectTransform DestroyItemContainer => _destroyItemHolder.ContainerHolder;
        public RectTransform FreeAreaItemContainer => _freeAreaItemHolder.ContainerHolder;
        public RectTransform ItemDragContainer => _itemDragContainer;
    }
}