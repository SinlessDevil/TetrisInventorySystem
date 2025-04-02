using System;
using Code.Utilities.Attributes;

namespace Code.InventoryModel.Data
{
    [Serializable]
    public class InventoryBalance
    {
        public InventoryBorders DefaultOpenedCells;
        public ItemId[] DefaultItems = Array.Empty<ItemId>();
        
        [Serializable]
        public class ItemId
        {
            [ItemIdSelector(HasGameObjectField = true)]
            public string Id;
        }
    }
}