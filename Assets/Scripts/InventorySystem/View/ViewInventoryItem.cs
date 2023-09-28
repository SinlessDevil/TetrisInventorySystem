using UnityEngine;
using UnityEngine.UI;
using InventorySystem.Abstract;
using Extensions;
using TMPro;

namespace InventorySystem.View
{
    public class ViewInventoryItem : MonoBehaviour
    {
        [SerializeField] private Image _imageIcon;
        [SerializeField] private TMP_Text _textAmount;

        public IInventoryItem Item { get; set; }

        private void Awake()
        {
            Assert();
        }
        private void Assert()
        {
            _imageIcon.LogErrorIfComponentNull();
            _textAmount.LogErrorIfComponentNull();
        }

        public void Refresh(IInventorySlot slot)
        {
            if (slot.IsEmpty)
            {
                CleanUp();
                return;
            }

            Item = slot.Item;
            _imageIcon.sprite = Item.Info.SpriteIcon;
            _textAmount.transform.Activate();

            var textAmountEnabled = slot.Amount > 1;
            _textAmount.gameObject.SetActive(textAmountEnabled);

            if (textAmountEnabled)
                _textAmount.text = $"x{slot.Amount}";
        }
        private void CleanUp()
        {
            _textAmount.transform.Deactivate();
            _imageIcon.transform.Deactivate();
        }
    }
}