using System;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Item
{
    public interface IItemViewModel
    {
        public event Action<IItemViewModel> StartedDragViewEvent;
        public event Action<Vector2, IItemViewModel> EndedDragViewEvent;
        public event Action<Vector2, IItemViewModel> ChangedPositionViewEvent;

        public event Action AnimationReturnToLastPositionEvent;
        public event Action<Quaternion> AnimationRotatedEvent;

        public event Action<IItemViewModel> EffectDropItemEvent;
        
        public InventoryModel.Items.Data.Item Item { get; }
        public RectTransform GetMainParent();
        public RectTransform GetDragParent();
        public Sprite GetItemSprite();
        public Sprite GetItemOutlineSprite();
        public Vector2 GetParentSize();
        public Vector2 GetRootSize();
        public Vector2 GetPivotPosition();
        public Vector2 GetPosition();
        public Vector2 GetRootPosition();
        public Quaternion GetGraphicRotation();
        public Vector3 GetGraphicFlipScale();
        public string GetTextCount();
        public Vector2 GetCountLevelPosition();
        
        public void SetStartPositionDrag(Vector3 position);
        public void SetEndPositionDrag(Vector3 position);
        public void SetPositionWhenDrag(Vector2 position);
        
        public void SetPosition(Vector2 position);
        
        public void PlayAnimationReturnToTargetPosition();
        public void PlayAnimationRotated(Quaternion rotation);
        
        public void PlayEffectDropItem();
    }
}