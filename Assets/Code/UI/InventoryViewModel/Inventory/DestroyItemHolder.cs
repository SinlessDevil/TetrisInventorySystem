using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Code.UI.InventoryViewModel.Inventory
{
    public class DestroyItemHolder : MonoBehaviour
    {
        [SerializeField] private RectTransform _destoryItemContainer;
        [SerializeField] private Image _glow;
        
        private Tween _glowTween;
        
        private IInventoryViewModel _inventoryVm;

        public void Initialize(IInventoryViewModel inventoryVM)
        {
            _inventoryVm = inventoryVM;
            
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }
        
        public RectTransform DestroyItemContainer => _destoryItemContainer;

        private void Subscribe()
        {
            _inventoryVm.EffectTogglePlayingDestroyGlowEvent += OnTogglePlayingGlowEffect;
        }

        private void Unsubscribe()
        {
            _inventoryVm.EffectTogglePlayingDestroyGlowEvent -= OnTogglePlayingGlowEffect;
        }

        private void OnTogglePlayingGlowEffect(bool isOn)
        {
            if(isOn)
                PlayGlowEffect();
            else
                StopGlowEffect();
        }

        private void PlayGlowEffect()
        {
            if(_glowTween != null)
                return;
            
            _glowTween?.Kill();
            
            var color = _glow.color;
            color.a = 0f;
            _glow.color = color;
            
            _glowTween = _glow
                .DOFade(1f, 0.8f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void StopGlowEffect()
        {
            if(_glowTween == null)
                return;
            
            _glowTween?.Kill();
            _glowTween = null;
            
            var color = _glow.color;
            color.a = 0f;
            _glow.color = color;
        }
    }
}