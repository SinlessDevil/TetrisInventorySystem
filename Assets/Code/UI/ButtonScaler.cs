using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;

namespace UI.Effects
{
    public class ButtonScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _rectTransform;

        [Header("Setup")]
        [SerializeField] private float ScaleAmount = 0.8f;
        [SerializeField] private float ScaleDuration = 0.2f;

        private Vector3 _originalScale;
        private CancellationTokenSource _scaleCts;

        private void OnValidate()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
        }

        private void Awake()
        {
            _originalScale = _rectTransform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _scaleCts?.Cancel();
            _scaleCts = new CancellationTokenSource();
            
            ScaleToAsync(_originalScale * ScaleAmount, ScaleDuration, _scaleCts.Token).Forget();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _scaleCts?.Cancel();
            _scaleCts = new CancellationTokenSource();
            
            ScaleToAsync(_originalScale, ScaleDuration, _scaleCts.Token).Forget();
        }

        private async UniTaskVoid ScaleToAsync(Vector3 targetScale, float duration, CancellationToken token)
        {
            float time = 0f;
            Vector3 startScale = _rectTransform.localScale;

            try
            {
                while (time < duration && !token.IsCancellationRequested)
                {
                    float t = time / duration;
                    _rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
                    time += Time.unscaledDeltaTime;
                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                if (!token.IsCancellationRequested)
                    _rectTransform.localScale = targetScale;
            }
            catch (OperationCanceledException) { }
        }

        private void OnEnable()
        {
            _rectTransform.localScale = _originalScale;
        }

        private void OnDisable()
        {
            _rectTransform.localScale = _originalScale;
            _scaleCts?.Cancel();
            _scaleCts = null;
        }
    }
}
