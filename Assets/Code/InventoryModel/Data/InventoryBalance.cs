using System;
using Code.Utilities.Attributes;
using UnityEngine;

namespace Code.InventoryModel.Data
{
    [Serializable]
    public class InventoryBalance
    {
        public InventoryBorders DefaultOpenedCells;
        public ItemId[] DefaultItems = Array.Empty<ItemId>();
        public Color[] RangColor = new Color[3];
        
        [Serializable]
        public class ItemId
        {
            [ItemIdSelector(HasGameObjectField = true)]
            public string Id;
        }
        
        [Serializable]
        public class DragItemTutorialBalance
        {
            [ItemIdSelector(HasGameObjectField = true)] 
            public string DragItemId;
        }
        
        [Serializable]
        public class MergeItemTutorialBalance
        {
            [ItemIdSelector(HasGameObjectField = true)]
            public string MergeItemId;
        }
    }
}