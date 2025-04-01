using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.InventoryModel.Data
{
    [CreateAssetMenu(menuName = "StaticData/Inventory/Expand", fileName = "InventoryExpandData", order = 800)]
    public class InventoryExpandData : ScriptableObject
    {
        [ListDrawerSettings(AddCopiesLastElement = true)]
        public InventoryExpandConfig[] Configs;
    }
}