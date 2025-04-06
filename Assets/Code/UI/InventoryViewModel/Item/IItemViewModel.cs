using System;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Item
{
    public interface IItemViewModel
    {
        event Action<IItemViewModel> StartedDragViewEvent;
        event Action<Vector2, IItemViewModel> EndedDragViewEvent;
        event Action<Vector2> ChangedPositionViewEvent;
        
        public InventoryModel.Items.Data.Item Item { get; }
        public RectTransform GetParent();
        public Sprite GetItemSprite();
        public Vector2 GetParentSize();
        public Vector2 GetRootSize();
        public Vector2 GetPivotPosition();
        public Vector2 GetPosition();
        public Vector2 GetRootPosition();
        public Quaternion GetGraphicRotation();
        public Vector3 GetGraphicFlipScale();
        
        void SetStartDrag(Vector3 position);
        void SetEndDrag(Vector3 position);
        void ChangPosition(Vector2 position);
    }
}