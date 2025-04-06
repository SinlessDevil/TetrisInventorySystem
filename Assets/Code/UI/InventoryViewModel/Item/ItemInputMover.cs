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
            RectTransform rootCenterRectTransform, 
            IItemViewModel itemViewModel)
        {
            _rootCenterRectTransform = rootCenterRectTransform;
            _itemVM = itemViewModel;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _offset = (Vector2)transform.position - eventData.position;
            _itemVM.SetStartDrag(_offset);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _itemVM.SetEndDrag(_rootCenterRectTransform.transform.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _itemVM.ChangPosition(eventData.position);
        }
    }
}