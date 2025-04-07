using System.Collections.Generic;
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
            SetLastSibling();
            
            _offset = (Vector2)transform.position - eventData.position;
            _itemVM.SetStartPositionDrag(_offset);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
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
    }
}