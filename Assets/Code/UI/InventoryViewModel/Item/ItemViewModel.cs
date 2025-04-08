using System;
using UnityEngine;
using Code.InventoryModel.Items.Provider;
using UI.Inventory;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemViewModel : IItemViewModel
    {
        private Vector2 _offset;

        private readonly InventoryModel.Items.Data.Item _item;
        private readonly IItemPositionFinding _itemPositionFinding;
        private readonly IItemDataProvider _itemDataProvider;
        private readonly RectTransform _parentRectTransform;
        private readonly int _sizeSlot;
        private readonly Vector2 _spawnPosition;
        private readonly Quaternion _spawnRotation;

        public ItemViewModel(
            InventoryModel.Items.Data.Item item,
            IItemPositionFinding itemPositionFinding,
            IItemDataProvider itemDataProvider,
            RectTransform parentRectTransform,
            int sizeSlot,
            Vector2 spawnPosition,
            Quaternion spawnRotation)
        {
            _item = item;
            _itemPositionFinding = itemPositionFinding;
            _itemDataProvider = itemDataProvider;
            _parentRectTransform = parentRectTransform;
            _sizeSlot = sizeSlot;
            _spawnPosition = spawnPosition;
            _spawnRotation = spawnRotation;
        }

        public event Action<IItemViewModel> StartedDragViewEvent;
        public event Action<Vector2, IItemViewModel> EndedDragViewEvent;
        public event Action<Vector2, IItemViewModel> ChangedPositionViewEvent;

        public event Action AnimationReturnToLastPositionEvent;
        public event Action<Quaternion> AnimationRotatedEvent;

        public event Action<IItemViewModel> EffectDropItemEvent;

        public InventoryModel.Items.Data.Item Item => _item;

        public RectTransform GetParent() => _parentRectTransform;

        public Sprite GetItemSprite() => _itemDataProvider.ForItemId(_item.Id).Item.Graphic.Icon;
        
        public Sprite GetItemOutlineSprite() => _itemDataProvider.ForItemId(_item.Id).Item.Graphic.IconOutline;

        public Vector2 GetParentSize() => new(_sizeSlot * _item.Graphic.Scale.x, _sizeSlot * _item.Graphic.Scale.y);

        public Vector2 GetRootSize() => new(_sizeSlot, _sizeSlot);

        public Vector2 GetPivotPosition() =>
            new(_sizeSlot * _item.Graphic.OffsetPivot.x * _item.Graphic.Scale.x,
                _sizeSlot * _item.Graphic.OffsetPivot.y * _item.Graphic.Scale.y);

        public Vector2 GetPosition() =>
            _itemPositionFinding.TryGetPositionItemById(_item.InstanceId) == false
                ? _spawnPosition
                : _itemPositionFinding.GetPositionItemInSlotById(_item.InstanceId);

        public Vector2 GetRootPosition() =>
            _itemPositionFinding.GetRootPositionByRootIndex(_item.Graphic.OffsetRoot.x,
                _item.Graphic.OffsetRoot.y);

        public Quaternion GetGraphicRotation() => _item.Graphic.Rotation * _spawnRotation;

        public Vector3 GetGraphicFlipScale() => _item.Graphic.FlipScale;

        #region Set Drags

        public void SetStartPositionDrag(Vector3 position)
        {
            _offset = (Vector2)position;
            StartedDragViewEvent?.Invoke(this);
        }

        public void SetEndPositionDrag(Vector3 position)
        {
            EndedDragViewEvent?.Invoke(position, this);
        }

        public void SetPositionWhenDrag(Vector2 position)
        {
            Vector3 newPosition = position + _offset;
            ChangedPositionViewEvent?.Invoke(newPosition, this);
        }

        #endregion

        #region Play Animation

        public void PlayAnimationReturnToTargetPosition()
        {
            AnimationReturnToLastPositionEvent?.Invoke();
        }

        public void PlayAnimationRotated(Quaternion rotation)
        {
            AnimationRotatedEvent?.Invoke(rotation);
        }

        #endregion

        #region Play Effect

        public void PlayEffectDropItem()
        {
            EffectDropItemEvent?.Invoke(this);
        }

        #endregion
    }
}