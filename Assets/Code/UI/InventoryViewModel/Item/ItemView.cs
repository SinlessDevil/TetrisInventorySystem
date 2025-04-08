using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private RectTransform _mainRectTransform;
        [SerializeField] private RectTransform _rootRectTransform;
        [SerializeField] private RectTransform _iconContainer;
        [Space(10)] [Header("Icons")]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _shadow;
        [SerializeField] private Image _outline;
        [SerializeField] private Image _goldOutline;
        [Space(10)] [Header("Additional Components")]
        [SerializeField] private ItemInputMover _itemInputMover;
        [SerializeField] private ItemAnimator _itemAnimator;
        [SerializeField] private ItemEffecter _itemEffecter;
        
        private IItemViewModel _viewModel;

        private void OnValidate()
        {
            if(_itemInputMover ==null)
                _itemInputMover = GetComponent<ItemInputMover>();
            
            if(_itemAnimator == null)
                _itemAnimator = GetComponent<ItemAnimator>();
            
            if(_itemEffecter == null)
                _itemEffecter = GetComponent<ItemEffecter>();
        }
        
        public void Initialize(IItemViewModel viewModel)
        {
            _viewModel = viewModel;
            
            SetSpriteIcon(_viewModel.GetItemSprite(), _viewModel.GetItemOutlineSprite());
            SetParent(_viewModel.GetMainParent());
            SetParentSize(_viewModel.GetParentSize());
            SetRootSize(_viewModel.GetRootSize());
            SetPivotPosition(_viewModel.GetPivotPosition());
            SetLocalPosition(_viewModel.GetPosition());
            SetRootCenterPosition(_viewModel.GetRootPosition());
            SetImageRotation(_viewModel.GetGraphicRotation());
            SetImageFlipScale(_viewModel.GetGraphicFlipScale());
            
            _itemInputMover.Initialize(_viewModel, _rootRectTransform);
            _itemAnimator.Initialize(_viewModel, _mainRectTransform, _iconContainer);
            _itemEffecter.Initialize(_viewModel, _icon);
            
            Subscribe();
        }

        public void Dispose()
        {
            _itemAnimator.Dispose();
            _itemEffecter.Dispose();
            
            Unsubscribe();
            
            Destroy(gameObject);
        }
        
        private void SetParent(RectTransform parent) => this.transform.SetParent(parent);

        private void SetSpriteIcon(Sprite icon, Sprite outline)
        {
            _icon.sprite = icon;
            _shadow.sprite = icon;
            _outline.sprite = outline;
            _goldOutline.sprite = outline;
        }

        private void SetParentSize(Vector2 size) => _mainRectTransform.sizeDelta = size;

        private void SetRootSize(Vector2 size) => _rootRectTransform.sizeDelta = size;

        private void SetPivotPosition(Vector2 pivotPosition) => _iconContainer.localPosition = pivotPosition;

        private void SetLocalPosition(Vector2 position) => _mainRectTransform.localPosition = position;

        private void SetRootCenterPosition(Vector2 position) => _rootRectTransform.localPosition = position;

        private void SetImageRotation(Quaternion rotation) => _iconContainer.rotation = rotation;

        private void SetImageFlipScale(Vector3 flipScale) => _iconContainer.localScale = flipScale;

        private void Subscribe()
        {
            _viewModel.ChangedPositionViewEvent += OnChangedPosition;
        }

        private void Unsubscribe()
        {
            _viewModel.ChangedPositionViewEvent -= OnChangedPosition;
        }
        
        private void OnChangedPosition(Vector2 newPosition, IItemViewModel viewModel)
        {
            _mainRectTransform.position = newPosition;
        }
    }
}