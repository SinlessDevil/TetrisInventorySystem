using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Effects
{
    public class ButtonScaler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [Header("Setup")]
        [SerializeField] private float ScaleAmount = 0.8f;
        [SerializeField] private float ScaleDuration = 0.2f;
    
        private Vector3 _originalScale;

        private void OnValidate()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
        }

        private void Awake() => _originalScale = _rectTransform.transform.localScale;

        public void OnPointerDown(PointerEventData eventData)
        {
            StopAllCoroutines();
            _rectTransform.transform.localScale = _originalScale;
            StartCoroutine(ScaleButton(_originalScale * ScaleAmount, ScaleDuration));
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            StopAllCoroutines();
            StartCoroutine(ScaleButton(_originalScale, ScaleDuration));
        }

        private IEnumerator ScaleButton(Vector3 targetScale, float duration)
        {
            float time = 0;
            Vector3 startScale = _rectTransform.transform.localScale;

            while (time < duration)
            {
                float t = time / duration;
                _rectTransform.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _rectTransform.transform.localScale = targetScale;
        }

        private void OnEnable()
        {
            _rectTransform.transform.localScale = _originalScale;
        }
        private void OnDisable()
        {
            _rectTransform.transform.localScale = _originalScale;
        }
    }   
}