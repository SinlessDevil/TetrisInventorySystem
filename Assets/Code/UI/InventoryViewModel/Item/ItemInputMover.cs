using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemInputMover : MonoBehaviour , IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 _offset;
        
        private IItemViewModel _itemVM;
        private RectTransform _rootCenterRectTransform;

        public void Initialize(
            IItemViewModel itemViewModel,
            RectTransform rootCenterRectTransform)
        {
            _rootCenterRectTransform = rootCenterRectTransform;
            _itemVM = itemViewModel;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            SetLastSibling();

            SetParent(_itemVM.GetDragParent());
            
            _offset = (Vector2)transform.position - eventData.position;
            _itemVM.SetStartPositionDrag(_offset);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            SetParent(_itemVM.GetMainParent());
            
            _itemVM.SetEndPositionDrag(_rootCenterRectTransform.transform.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _itemVM.SetPositionWhenDrag(eventData.position);
        }

        private void SetLastSibling()
        {
            this.transform.SetAsLastSibling();
        }
        
        private void SetParent(Transform transform)
        {
            this.transform.SetParent(transform);
        }
    }
}