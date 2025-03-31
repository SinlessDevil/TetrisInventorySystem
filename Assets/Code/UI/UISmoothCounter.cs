using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISmoothCounter : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private float _endValue;

        private Coroutine _coroutine;

        public void TextSmoothTransition(int endValue, float durationPerIndex = 0.02f, bool scaling = true)
        {
            _endValue = endValue;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (gameObject.activeInHierarchy)
                _coroutine = StartCoroutine(SmoothTransitionRoutine(durationPerIndex, scaling));
            else
                _text.text = _endValue.ToString();
        }

        private IEnumerator SmoothTransitionRoutine(float durationPerIndex, bool scaling)
        {
            float t = 0f;

            while (t <= _endValue)
            {
                t += 1 / Mathf.Abs(int.Parse(_text.text) - _endValue);
                int currentValue = Mathf.RoundToInt(Mathf.Lerp(int.Parse(_text.text), _endValue, t));
                _text.text = currentValue.ToString();
                
                if (scaling)
                    yield return StartCoroutine(LittleScaleRoutine(durationPerIndex));
                else
                    yield return new WaitForSeconds(durationPerIndex);

                yield return null;
            }

            _text.text = _endValue.ToString();
        }

        private IEnumerator LittleScaleRoutine(float durationPerIndex)
        {
            Vector3 originalScale = transform.localScale;
            Vector3 targetScale = transform.localScale * 1.1f;

            float timer = 0f;
            while (timer < durationPerIndex)
            {
                timer += Time.deltaTime;
                float t = timer / durationPerIndex;
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;
            }

            timer = 0f;
            while (timer < durationPerIndex)
            {
                timer += Time.deltaTime;
                float t = timer / durationPerIndex;
                transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
                yield return null;
            }

            transform.localScale = originalScale;
        }
    }
}