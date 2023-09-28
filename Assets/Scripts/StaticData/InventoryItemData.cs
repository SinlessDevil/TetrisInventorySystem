using UnityEngine;
using InventorySystem.Abstract;

namespace InventorySystem.Model.Items.StaticData
{
    [CreateAssetMenu(fileName = "InventoryItemData", menuName = "Gameplay/items/Create new item data")]
    public class InventoryItemData : ScriptableObject, IInventoryItemInfo
    {
        [SerializeField] private string _id;
        [SerializeField] private string _title;
        [SerializeField] private string _description;
        [SerializeField] private int _maxItemsInInventorySlot;
        [SerializeField] private Sprite _spriteIcon;

        public string Id => _id;
        public string Title => _title;
        public string Description => _description;
        public int MaxItemsInInventorySlot => _maxItemsInInventorySlot;
        public Sprite SpriteIcon => _spriteIcon;
    }
}