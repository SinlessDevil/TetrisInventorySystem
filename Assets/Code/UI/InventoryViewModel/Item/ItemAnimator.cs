using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemAnimator : MonoBehaviour
    {
        private const float _duration = 0.175f;
        
        [SerializeField] private ReturnItemAnimationPreset _returnItemAnimationPreset;

        private CancellationTokenSource _returnCts;
        private CancellationTokenSource _rotationCts;

        private IItemViewModel _itemVM;
        private RectTransform _mainRectTransform;
        private RectTransform _iconContainer;

        public void Initialize(
            IItemViewModel itemViewModel,
            RectTransform mainRectTransform,
            RectTransform iconContainer)
        {
            _iconContainer = iconContainer;
            _itemVM = itemViewModel;
            _mainRectTransform = mainRectTransform;

            Subscribe();
        }

        public void Dispose()
        {
            _returnCts?.Cancel();
            _rotationCts?.Cancel();
            
            Unsubscribe();
        }

        private void SetLocalPosition(Vector2 position) =>
            _mainRectTransform.localPosition = position;

        private void Subscribe()
        {
            _itemVM.AnimationReturnToLastPositionEvent += OnAnimationReturnToLastPositionWrap;
            _itemVM.AnimationRotatedEvent += OnAnimationRotationWrap;
        }

        private void Unsubscribe()
        {
            _itemVM.AnimationReturnToLastPositionEvent -= OnAnimationReturnToLastPositionWrap;
            _itemVM.AnimationRotatedEvent -= OnAnimationRotationWrap;
        }

        private void OnAnimationReturnToLastPositionWrap()
        {
            _returnCts?.Cancel();
            _returnCts = new CancellationTokenSource();
            AnimationReturnToLastPositionAsync(_returnCts.Token).Forget();
        }

        private void OnAnimationRotationWrap(Quaternion targetRotation)
        {
            _rotationCts?.Cancel();
            _rotationCts = new CancellationTokenSource();
            AnimationRotationAsync(targetRotation, _rotationCts.Token).Forget();
        }

        private async UniTaskVoid AnimationReturnToLastPositionAsync(CancellationToken token)
        {
            Vector2 targetPosition = _itemVM.GetPosition();
            Vector3 startPosition = transform.localPosition;
            float time = 0f;

            float baseDuration = _returnItemAnimationPreset.TargetTime;
            float distanceFactor = Vector3.Distance(targetPosition, startPosition) / 500f;
            float duration = baseDuration * (1f + distanceFactor);

            try
            {
                while (time < duration && !token.IsCancellationRequested)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / duration);
                    Vector2 pos = Vector2.Lerp(startPosition, targetPosition, _returnItemAnimationPreset.Curve.Evaluate(t));
                    SetLocalPosition(pos);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                SetLocalPosition(targetPosition);
                _itemVM.PlayEffectDropItem();
            }
            catch (OperationCanceledException) { }
        }

        private async UniTaskVoid AnimationRotationAsync(Quaternion targetRotation, CancellationToken token)
        {
            Quaternion startRotation = _iconContainer.rotation;
            float time = 0f;
            
            try
            {
                while (time < _duration && !token.IsCancellationRequested)
                {
                    time += Time.deltaTime;
                    float t = Mathf.Clamp01(time / _duration);
                    _iconContainer.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                _iconContainer.rotation = targetRotation;
            }
            catch (OperationCanceledException) { }
        }
    }
}