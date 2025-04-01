using System;
using Sirenix.OdinInspector;

namespace Code.InventoryModel.Data
{
    [Serializable]
    public class InventoryPointsConfig
    {
        [HorizontalGroup(LabelWidth = 110)]
        [LabelText("For Completion Lvl")]
        public int Level;
        [HorizontalGroup(LabelWidth = 10)]
        [LabelText("+")]
        [SuffixLabel("points")]
        public int AdditionalPoints;
    }
}