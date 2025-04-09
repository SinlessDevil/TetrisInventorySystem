using System;
using System.Collections;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemAnimator : MonoBehaviour
    {
        [SerializeField] private ReturnItemAnimationPreset _returnItemAnimationPreset;
        
        private Coroutine _animationReturnToLastPositionCoroutine;
        private Coroutine _animationRotationCoroutine;

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
            if(_animationReturnToLastPositionCoroutine != null)
                StopCoroutine(AnimationReturnToLastPositionRoutine());
            
            _animationReturnToLastPositionCoroutine = StartCoroutine(AnimationReturnToLastPositionRoutine());
        }

        private void OnAnimationRotationWrap(Quaternion targetRotation)
        {
            if(_animationRotationCoroutine != null)
                StopCoroutine(AnimationRotationRoutine(targetRotation));
            
            _animationRotationCoroutine = StartCoroutine(AnimationRotationRoutine(targetRotation));
        }
        
        private IEnumerator AnimationReturnToLastPositionRoutine()
        {
            var targetPosition = _itemVM.GetPosition();
            var startPosition = transform.localPosition;
            var time = 0f;
            var targetTime = _returnItemAnimationPreset.TargetTime;
            targetTime *= 1 + (Vector3.Distance(targetPosition, startPosition) / 500);
            while (time < targetTime)
            {
                time += Time.deltaTime;
                var value = Mathf.Clamp01(time / targetTime);
                var pos = 
                    Vector2.Lerp(startPosition, targetPosition, _returnItemAnimationPreset.Curve.Evaluate(value));
                SetLocalPosition(pos);
                yield return typeof(WaitForEndOfFrame);
            }
            _itemVM.PlayEffectDropItem();
            _animationReturnToLastPositionCoroutine = null;
        }
        
        private IEnumerator AnimationRotationRoutine(Quaternion targetRotation)
        {
            var startRotation = _iconContainer.rotation;
            var time = 0f;
            var duration = 0.175f;
            while (time < duration)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / duration);
                _iconContainer.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                yield return null;
            }
            _iconContainer.rotation = targetRotation;
            _animationReturnToLastPositionCoroutine = null;
        }
    }

    [Serializable]
    public class ReturnItemAnimationPreset
    {
        public float TargetTime;
        public AnimationCurve Curve;
    }
}