using System;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Item
{
    public interface IItemViewModel
    {
        event Action<IItemViewModel> StartedDragViewEvent;
        event Action<Vector2, IItemViewModel> EndedDragViewEvent;
        event Action<Vector2, IItemViewModel> ChangedPositionViewEvent;

        event Action AnimationReturnToLastPositionEvent;
        event Action<Quaternion> AnimationRotatedEvent;

        event Action<IItemViewModel> EffectDropItemEvent;
        
        public InventoryModel.Items.Data.Item Item { get; }
        public RectTransform GetParent();
        public Sprite GetItemSprite();
        public Sprite GetItemOutlineSprite();
        public Vector2 GetParentSize();
        public Vector2 GetRootSize();
        public Vector2 GetPivotPosition();
        public Vector2 GetPosition();
        public Vector2 GetRootPosition();
        public Quaternion GetGraphicRotation();
        public Vector3 GetGraphicFlipScale();
        
        void SetStartPositionDrag(Vector3 position);
        void SetEndPositionDrag(Vector3 position);
        void SetPositionWhenDrag(Vector2 position);
        void PlayAnimationReturnToTargetPosition();
        void PlayAnimationRotated(Quaternion rotation);
        void PlayEffectDropItem();
    }
}