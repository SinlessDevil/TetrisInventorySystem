using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.InventoryViewModel.Slot
{
    public class SlotColorIntaractable : MonoBehaviour 
    {
        [Space(10)] [Header("Sprites Color Intecractable Slot")] 
        [SerializeField] private Sprite _colorFree;
        [SerializeField] private Sprite _colorNotFree;
        [SerializeField] private Sprite _colorFreeToPlaceItem;
        [SerializeField] private Sprite _colorBlockedPlaceItem;
        
        private ISlotViewModel _slotVm;
        private Image _unlocked;

        private bool _isResetedStateUnlocked;
        
        public void Initialize(
            ISlotViewModel slotVM,
            Image unlocked)
        {
            _unlocked = unlocked;
            _slotVm = slotVM;
            
            SetColorFilled(_slotVm.GetColorLockedSlot());
            
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void SetColorFilled(bool isFree)
        {
            if (_isResetedStateUnlocked)
            {
                _unlocked.sprite = _colorFree;
                return;
            }
            
            _unlocked.sprite = isFree ? _colorFree : _colorNotFree;
        }
        
        private void SetColorReaction(bool isCanPlaceItem)
        {
            _unlocked.sprite = isCanPlaceItem ? _colorFreeToPlaceItem :_colorBlockedPlaceItem;
        }
        
        private void Subscribe()
        {
            _slotVm.ColoredFillSlotEvent += SetColorFilled;
            _slotVm.ColoredReactionSlotEvent += SetColorReaction;
        }

        private void Unsubscribe()
        {
            _slotVm.ColoredFillSlotEvent -= SetColorFilled;
            _slotVm.ColoredReactionSlotEvent -= SetColorReaction;
        }
    }
}