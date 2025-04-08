using System;
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
        [Space(10)] [Header("Additional")]
        [SerializeField] private SlotColorIntaractable _slotColorIntaractable;
        [SerializeField] private SlotEffecter _slotEffecter;
        
        private ISlotViewModel _slotVM;

        private void OnValidate()
        {
            if(_slotColorIntaractable == null)
                _slotColorIntaractable = GetComponent<SlotColorIntaractable>();
            
            if(_slotEffecter == null)
                _slotEffecter = GetComponent<SlotEffecter>();
        }

        public void Initialize(ISlotViewModel viewModel)
        {
            _slotVM = viewModel;
            
            _slotColorIntaractable.Initialize(viewModel, _unlocked);
            _slotEffecter.Initialize(viewModel);
            
            SetInteractableButton(_slotVM.IsInteractableButton());
            SetTextLevel(_slotVM.GetTextLevel());
            SetSpriteForLockedState(_slotVM.HasNecessaryLevel(),_slotVM.IsLockedSlotAndIsAvailableToBuy());
            SetSlotState(_slotVM.IsUnlockedSlot() ,_slotVM.IsLockedSlot());
            
            Subscribe();
        }

        public void Dispose()
        {
            _slotColorIntaractable.Dispose();
            _slotEffecter.Dispose();
            
            Unsubscribe();
            
            Destroy(gameObject);
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
            if (hasNecessaryLevel)
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

        private void Subscribe()
        {
            _slotVM.ChangedStateSlotEvent += OnChangedStateSlot;
        }

        private void Unsubscribe()
        {
            _slotVM.ChangedStateSlotEvent -= OnChangedStateSlot;
        }
        
        private void OnChangedStateSlot()
        {
            SetInteractableButton(_slotVM.IsInteractableButton());
            SetSpriteForLockedState(_slotVM.HasNecessaryLevel(),_slotVM.IsLockedSlotAndIsAvailableToBuy());
            SetSlotState(_slotVM.IsUnlockedSlot() ,_slotVM.IsLockedSlot());
        }
    }
}