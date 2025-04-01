using Code.InventoryModel.Data;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Balance", fileName = "Balance", order = 0)]
    public class BalanceStaticData : ScriptableObject
    {
        public InventoryBalance Inventory = new InventoryBalance();
    }
}