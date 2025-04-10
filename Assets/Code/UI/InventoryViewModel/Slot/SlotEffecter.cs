using UnityEngine;
using DG.Tweening;

namespace Code.UI.InventoryViewModel.Slot
{
    public class SlotEffecter : MonoBehaviour
    {
        [SerializeField] private RectTransform _slotDropGlowPrefab;
        
        private ISlotViewModel _slotVM;
        
        public void Initialize(ISlotViewModel slotVM)
        {
            _slotVM = slotVM;
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _slotVM.EffectFilledSlotEvent += PlayEffectSlotFilledAnimation;
        }

        private void Unsubscribe()
        {
            _slotVM.EffectFilledSlotEvent -= PlayEffectSlotFilledAnimation;
        }

        private void PlayEffectSlotFilledAnimation()
        {
            var effect = GetDropAnimation();
            effect.transform.SetParent(transform);
        }
        
        private RectTransform GetDropAnimation()
        {
            var dropAnimationEffect = Instantiate(_slotDropGlowPrefab);
            dropAnimationEffect.position = transform.position;
            dropAnimationEffect.sizeDelta = GetComponent<RectTransform>().sizeDelta;
            return dropAnimationEffect;
        }
    }
}