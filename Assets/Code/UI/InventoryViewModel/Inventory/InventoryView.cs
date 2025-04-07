using UnityEngine;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private RectTransform _slotsContainer;
        [SerializeField] private RectTransform _itemsContainer;
        
        public RectTransform SlotsContainer => _slotsContainer;
        public RectTransform ItemsContainer => _itemsContainer;

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            Destroy(this.gameObject);
        }
    }
}