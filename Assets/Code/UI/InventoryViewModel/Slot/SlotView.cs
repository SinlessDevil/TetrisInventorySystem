using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InventoryViewModel.Slot
{
    public class SlotView : MonoBehaviour
    {
        [Space(10)] [Header("Main")] 
        [SerializeField] private Image _unlocked;
        [SerializeField] private Image _locked;
        [SerializeField] private CanvasGroup _perentCanvasGroup;
        [SerializeField] private Button _buttonForUnlockedSlots;
        [SerializeField] private Text _textLevel;
        [Space(10)] [Header("Sprites State Slot")] 
        [SerializeField] private Sprite _spriteLockedToOpen;
        [SerializeField] private Sprite _spiteLockedDontOpen;
        [SerializeField] private Sprite _spiteLockedDontOpenWithLevel;
        [Space(10)] [Header("Sprites Color Intecractable Slot")] 
        [SerializeField] private Sprite _colorFree;
        [SerializeField] private Sprite _colorNotFree;
        [SerializeField] private Sprite _colorFreeToPlaceItem;
        [SerializeField] private Sprite _colorBlockedPlaceItem;
        
        private ISlotViewModel _slotVM;
        
        public void Initialize(ISlotViewModel viewModel)
        {
            _slotVM = viewModel;
            
            SetInteractableButton(_slotVM.IsInteractableButton());
            SetTextLevel(_slotVM.GetTextLevel());
            SetSpriteForLockedState(_slotVM.HasNecessaryLevel(),_slotVM.IsLockedSlotAndIsAvailableToBuy());
            SetSlotState(_slotVM.IsUnlockedSlot() ,_slotVM.IsLockedSlot());
        }

        public void Dispose()
        {
            
        }
        
        private void SetInteractableButton(bool isInteractable)
        {
            _buttonForUnlockedSlots.interactable = isInteractable;
        }
        
        private void SetTextLevel(string text)
        {
            _textLevel.text = text;
        }
        
        private void SetSpriteForLockedState(bool hasNecessaryLevel, bool isValidate)
        {
            if (!hasNecessaryLevel)
            {
                _locked.sprite = _spiteLockedDontOpenWithLevel;
                return;
            }
            
            _locked.sprite = isValidate ? _spriteLockedToOpen : _spiteLockedDontOpen;
        }
        
        private void SetSlotState(bool isUnlocked, bool isLocked)
        {
            _unlocked.gameObject.SetActive(isUnlocked);
            _locked.gameObject.SetActive(isLocked);
        }
    }
}