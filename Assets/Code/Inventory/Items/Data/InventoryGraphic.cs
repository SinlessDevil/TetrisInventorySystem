using System;
using UnityEngine;

namespace Code.Inventory.Items.Data
{
    [Serializable]
    public class InventoryGraphic
    {
        public Sprite Icon;
        public Sprite IconOutline;
        public Vector2 OffsetPivot = new Vector2(0.5f, 0.5f);
        public Vector2 OffsetRoot = new Vector2(0, 0);
        public Vector2 OffsetIconLevel = new Vector2(0f, 0f);
        public Vector2 Scale = Vector2.one;
        public Quaternion Rotation = Quaternion.identity;
        public Vector3 FlipScale = Vector3.one;
    }
}