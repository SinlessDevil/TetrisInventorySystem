using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private RectTransform _mainRectTransform;
        [SerializeField] private RectTransform _rootRectTransform;
        [SerializeField] private RectTransform _iconContainer;
        [Space(10)] [Header("Icon Count")]
        [SerializeField] private RectTransform _iconCountRectTransform;
        [SerializeField] private Text _textCount;
        [Space(10)] [Header("Icons")]
        [SerializeField] private Image _icon;
        [SerializeField] private Image _shadow;
        [SerializeField] private Image _outline;
        [SerializeField] private Image _goldOutline;
        [Space(10)] [Header("Additional Components")]
        [SerializeField] private ItemInputMover _itemInputMover;
        [SerializeField] private ItemAnimator _itemAnimator;
        [SerializeField] private ItemEffecter _itemEffecter;
        
        private IItemViewModel _itemVM;

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
            _itemVM = viewModel;
            
            SetSpriteIcon(_itemVM.GetItemSprite(), _itemVM.GetItemOutlineSprite());
            SetParent(_itemVM.GetMainParent());
            SetParentSize(_itemVM.GetParentSize());
            SetRootSize(_itemVM.GetRootSize());
            SetPivotPosition(_itemVM.GetPivotPosition());
            SetLocalPosition(_itemVM.GetPosition());
            SetRootCenterPosition(_itemVM.GetRootPosition());
            SetImageRotation(_itemVM.GetGraphicRotation());
            SetImageFlipScale(_itemVM.GetGraphicFlipScale());
            SetTextCount(_itemVM.GetTextCount());
            SetIconCountPosition(_itemVM.GetCountLevelPosition());
            
            _itemInputMover.Initialize(_itemVM, _rootRectTransform);
            _itemAnimator.Initialize(_itemVM, _mainRectTransform, _iconContainer);
            _itemEffecter.Initialize(_itemVM, _icon);
            
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

        private void SetTextCount(string count) => _textCount.text = count;
        private void SetIconCountPosition(Vector2 position) => _iconCountRectTransform.localPosition = position;
        
        private void Subscribe()
        {
            _itemVM.ChangedPositionViewEvent += OnChangedPosition;
            _itemVM.EffectStackItemEvent += OnUpdateCount;
        }
        
        private void Unsubscribe()
        {
            _itemVM.ChangedPositionViewEvent -= OnChangedPosition;
            _itemVM.EffectStackItemEvent -= OnUpdateCount;
        }
        
        private void OnChangedPosition(Vector2 newPosition, IItemViewModel viewModel)
        {
            _mainRectTransform.position = newPosition;
        }
        
        private void OnUpdateCount()
        {
            SetTextCount(_itemVM.GetTextCount());
        }
    }
}