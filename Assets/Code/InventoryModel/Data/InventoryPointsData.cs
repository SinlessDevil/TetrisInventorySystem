using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.InventoryModel.Data
{
    [CreateAssetMenu(menuName = "StaticData/Inventory/InventoryPointsData", fileName = "InventoryPointsData", order = 801)]
    public class InventoryPointsData : ScriptableObject
    {
        [ListDrawerSettings(AddCopiesLastElement = true)]
        public InventoryPointsConfig[] Configs;
    }
}