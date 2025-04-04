using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private RectTransform _mainRectTransform;
        [SerializeField] private RectTransform _rootRectTransform;
        [SerializeField] private RectTransform _iconContainer;
        [Space(10)]
        [SerializeField] private Image _icon;
        
        private IItemViewModel _viewModel;
        
        public void Initialize(IItemViewModel viewModel)
        {
            _viewModel = viewModel;
            
            SetParent(_viewModel.GetParent());
            SetSpriteIcon(_viewModel.GetItemSprite());
            SetParentSize(_viewModel.GetParentSize());
            SetRootSize(_viewModel.GetRootSize());
            SetPivotPosition(_viewModel.GetPivotPosition());
            SetLocalPosition(_viewModel.GetPosition());
            SetRootCenterPosition(_viewModel.GetRootPosition());
            SetImageRotation(_viewModel.GetGraphicRotation());
            SetImageFlipScale(_viewModel.GetGraphicFlipScale());
        }

        private void SetParent(RectTransform parent)
        {
            this.transform.SetParent(parent);
        }

        private void SetSpriteIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        private void SetParentSize(Vector2 size)
        {
            _mainRectTransform.sizeDelta = size;
        }

        private void SetRootSize(Vector2 size)
        {
            _rootRectTransform.sizeDelta = size;
        }
        
        private void SetPivotPosition(Vector2 pivotPosition)
        {
            _iconContainer.localPosition = pivotPosition;
        }

        private void SetLocalPosition(Vector2 position)
        {
            _mainRectTransform.localPosition = position;
        }
        
        private void SetRootCenterPosition(Vector2 position)
        {
            _rootRectTransform.localPosition = position;
        }

        private void SetImageRotation(Quaternion rotation)
        {
            _iconContainer.rotation = rotation;
        }

        private void SetImageFlipScale(Vector3 flipScale)
        {
            _iconContainer.localScale = flipScale;
        }
    }
}