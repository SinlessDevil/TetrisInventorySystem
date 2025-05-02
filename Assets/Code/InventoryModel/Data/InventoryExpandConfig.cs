using System;
using Sirenix.OdinInspector;

namespace Code.InventoryModel.Data
{
    [Serializable]
    public class InventoryExpandConfig
    {
        [HorizontalGroup]
        public int Level;
        [HorizontalGroup, InlineEditor]
        public InventoryBorders Borders;
    }
}