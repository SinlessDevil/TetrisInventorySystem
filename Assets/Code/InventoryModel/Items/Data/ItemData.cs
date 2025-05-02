using Sirenix.OdinInspector;
using UnityEngine;

namespace Code.InventoryModel.Items.Data
{
    [CreateAssetMenu(menuName = "StaticData/Inventory/Item", fileName = "Item", order = 500)]
    public class ItemData : SerializedScriptableObject
    {
        public Item Item;

        [PropertyOrder(-1)]
        [ShowInInspector, ShowIf("@Item!=null")]
        [ValidateInput("@Id!=string.Empty", "Id is null or empty")]
        public string Id
        {
            get => Item != null ? Item.Id : string.Empty;
            set => Item.Id = value;
        }
    }
}