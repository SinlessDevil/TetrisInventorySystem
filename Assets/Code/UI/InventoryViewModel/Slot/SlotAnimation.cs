using DG.Tweening;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Slot
{
    public class SlotAnimation : MonoBehaviour
    {
        private ISlotViewModel _viewModel;

        public void Initialize(ISlotViewModel viewModel)
        {
            _viewModel = viewModel;
            Subscribe();
        }
        
        public void Dispose()
        {
            _viewModel = null;
            Unsubscribe();
        }

        private void Subscribe()
        {
            _viewModel.TryUnlockedSlotEvent += PlayAnimationClicked;
        }

        private void Unsubscribe()
        {
            _viewModel.TryUnlockedSlotEvent -= PlayAnimationClicked;
        }
        
        private void PlayAnimationClicked()
        {
            var targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            var endScale = Vector3.one;
            gameObject.transform.DOScale(targetScale, 0.15f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    gameObject.transform.DOScale(endScale, 0.15f)
                        .SetEase(Ease.Linear);
                });
        }
    }
}