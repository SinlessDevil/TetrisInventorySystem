using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Code.UI.InventoryViewModel.Inventory
{
    public abstract class ContainerDropHolder : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransformHolder;
        [SerializeField] private Image _glow;
        
        private bool _isPlayingGlow;
        
        protected IInventoryViewModel _inventoryVm;

        public void Initialize(IInventoryViewModel inventoryVM)
        {
            _inventoryVm = inventoryVM;
            
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }
        
        public RectTransform ContainerHolder => rectTransformHolder;

        protected abstract void Subscribe();

        protected abstract void Unsubscribe();

        protected void OnTogglePlayingGlowEffect(bool isOn)
        {
            if(isOn)
                PlayGlowEffect();
            else
                StopGlowEffect();
        }

        private void PlayGlowEffect()
        {
            if (_isPlayingGlow)
                return;
            
            _isPlayingGlow = true;
            
            var color = _glow.color;
            color.a = 0f;
            _glow.color = color;
            
            _glow.DOFade(1f, 0.8f)
                .SetEase(Ease.InOutSine);
        }

        private void StopGlowEffect()
        {
            if(!_isPlayingGlow)
                return;
            
            _isPlayingGlow = false;
            
            _glow.DOFade(0f, 0.8f)
                .SetEase(Ease.InOutSine);
        }
    }
}