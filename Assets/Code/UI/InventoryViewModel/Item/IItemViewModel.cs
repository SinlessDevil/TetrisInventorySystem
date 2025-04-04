using UnityEngine;

namespace Code.UI.InventoryViewModel.Item
{
    public interface IItemViewModel
    {
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
    }
}