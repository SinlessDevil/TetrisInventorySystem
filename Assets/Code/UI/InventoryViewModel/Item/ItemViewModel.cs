using Code.InventoryModel.Items.Provider;
using UI.Inventory;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemViewModel : IItemViewModel
    {
        private readonly InventoryModel.Items.Data.Item _item;
        private readonly IItemPositionFinding _itemPositionFinding;
        private readonly RectTransform _parentRectTransform;
        private readonly IItemDataProvider _itemDataProvider;
        private readonly int _sizeSlot;
        private readonly Vector2 _spawnPosition;
        private readonly Quaternion _spawnRotation;

        public ItemViewModel(
            InventoryModel.Items.Data.Item item, 
            IItemPositionFinding itemPositionFinding,
            RectTransform parentRectTransform,
            IItemDataProvider itemDataProvider,
            int sizeSlot,
            Vector2 spawnPosition,
            Quaternion spawnRotation)
        {
            _item = item;
            _itemPositionFinding = itemPositionFinding;
            _parentRectTransform = parentRectTransform;
            _itemDataProvider = itemDataProvider;
            _sizeSlot = sizeSlot;
            _spawnPosition = spawnPosition;
            _spawnRotation = spawnRotation;
        }
        
        public InventoryModel.Items.Data.Item Item => _item;
        
        public RectTransform GetParent()
        {
            return _parentRectTransform;
        }

        public Sprite GetItemSprite()
        {
            return _itemDataProvider.ForItemId(_item.Id).Item.Graphic.Icon;
        }

        public Vector2 GetParentSize()
        {
            return new Vector2(_sizeSlot * _item.Graphic.Scale.x, _sizeSlot * _item.Graphic.Scale.y);
        }

        public Vector2 GetRootSize()
        {
            return new Vector2(_sizeSlot, _sizeSlot);
        }

        public Vector2 GetPivotPosition()
        {
            return new Vector2(_sizeSlot * _item.Graphic.OffsetPivot.x * _item.Graphic.Scale.x, 
                _sizeSlot * _item.Graphic.OffsetPivot.y * _item.Graphic.Scale.y);
        }

        public Vector2 GetPosition()
        {
            if (_itemPositionFinding.TryGetPositionItemById(_item.InstanceId) == false)
            {
                return _spawnPosition;
            }
            return _itemPositionFinding.GetPositionItemInSlotById(_item.InstanceId);
        }

        public Vector2 GetRootPosition()
        {
            return _itemPositionFinding.GetRootPositionByRootIndex(_item.Graphic.OffsetRoot.x, 
                _item.Graphic.OffsetRoot.y);
        }

        public Quaternion GetGraphicRotation()
        {
            return _item.Graphic.Rotation * _spawnRotation;
        }

        public Vector3 GetGraphicFlipScale()
        {
            return _item.Graphic.FlipScale;
        }
    }
}