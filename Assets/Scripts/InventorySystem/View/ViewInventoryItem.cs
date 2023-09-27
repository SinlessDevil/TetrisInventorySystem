using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InventorySystem.Abstract;

namespace InventorySystem.View
{
    public class ViewInventoryItem : MonoBehaviour
    {
        [SerializeField] private Image _imageIcon;
        [SerializeField] private TMP_Text _textAmount;

        public IInventoryItem item { get; set; }

        public void Refresh(IInventorySlot slot)
        {
            if (slot.IsEmpty)
            {
                CleanUp();
                return;
            }

            item = slot.Item;
            _imageIcon.sprite = item.Info.SpriteIcon;

            var textAmountEnabled = slot.Amount > 1;
            _textAmount.gameObject.SetActive(textAmountEnabled);

            if (textAmountEnabled)
                _textAmount.text = $"x{slot.Amount}";
        }
        private void CleanUp()
        {
            _textAmount.gameObject.SetActive(false);
            _imageIcon.gameObject.SetActive(false);
        }
    }
}