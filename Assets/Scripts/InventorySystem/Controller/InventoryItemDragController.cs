using UnityEngine;
using UnityEngine.EventSystems;
using Extensions;

namespace InventorySystem.Controller
{
    public class InventoryItemDragController : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        private Canvas _mainCanvas;
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            InitComponent();
            Asserts();
        }
        private void InitComponent()
        {
            _rectTransform = GetComponent<RectTransform>();
            _mainCanvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        private void Asserts()
        {
            _rectTransform.LogErrorIfComponentNull();
            _mainCanvas.LogErrorIfComponentNull();
            _canvasGroup.LogErrorIfComponentNull();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var slotTransfrom = _rectTransform.parent;
            slotTransfrom.SetAsLastSibling();
            _canvasGroup.blocksRaycasts = false;
        }
        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            transform.localPosition = Vector3.zero;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}