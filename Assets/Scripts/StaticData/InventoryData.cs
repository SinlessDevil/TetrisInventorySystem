using UnityEngine;

namespace InventorySystem.Model.Items.StaticData
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Gameplay/inventory")]
    public class InventoryData : ScriptableObject
    {
        public int Width;
        public int Height;
        public SlotData[,] Inventory;
    }

    [System.Serializable]
    public class SlotData
    {
        public TypeItem ItemType;
        public int CurrentAmountItem;
        public bool Foldout;
    }
}