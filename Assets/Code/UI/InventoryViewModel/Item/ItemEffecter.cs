using System;
using System.Threading;
using Code.InventoryModel;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;
using Cysharp.Threading.Tasks;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemEffecter : MonoBehaviour
    {
        private static readonly int EmissionValueID = Shader.PropertyToID("_Emission");
        private static readonly int GlowColorID = Shader.PropertyToID("_GlowColor");

        [Header("Main")]
        [SerializeField] private Image _outlineImage;
        [SerializeField] private Image _goldOutlineImage;

        [Header("Effects")]
        [SerializeField] private UIParticle _particlePrefab;

        [Header("Glow Presets")]
        [SerializeField] private GlowPreset _mergeGlow;
        [SerializeField] private GlowPreset _dropGlow;
        [SerializeField] private GlowPreset _sameItemGlow;

        private Material _glowMaterial;
        private IItemViewModel _itemVM;
        private Image _icon;

        private CancellationTokenSource _highlightCts;

        public void Initialize(IItemViewModel itemVM, Image icon)
        {
            _itemVM = itemVM;
            _icon = icon;

            SetUpGlowMaterial(icon);

            _icon.material = null;
            
            SetGlowValue(0);
            ToggleGoldOutline(false);

            Subscribe();
        }

        private void SetUpGlowMaterial(Image icon)
        {
            _glowMaterial = Instantiate(icon.material);
            _glowMaterial.name = icon.material.name + "_Clone";
        }

        public void Dispose()
        {
            _highlightCts?.Cancel();
            
            Unsubscribe();
        }

        private void Subscribe()
        {
            _itemVM.EffectDropItemEvent += OnDropEffect;
            _itemVM.EffectStackItemEvent += OnStackEffect;
            _itemVM.EffectStartOutlineGlowEvent += OnStartHighlight;
            _itemVM.EffectEndOutlineGlowEvent += OnEndHighlight;
        }

        private void Unsubscribe()
        {
            _itemVM.EffectDropItemEvent -= OnDropEffect;
            _itemVM.EffectStackItemEvent -= OnStackEffect;
            _itemVM.EffectStartOutlineGlowEvent -= OnStartHighlight;
            _itemVM.EffectEndOutlineGlowEvent -= OnEndHighlight;
        }

        private void OnDropEffect(IItemViewModel _) => PlayGlow(_dropGlow);

        private void OnStackEffect()
        {
            SpawnParticles();
            PlayGlow(_mergeGlow);
        }

        private void OnStartHighlight()
        {
            _highlightCts?.Cancel();
            _highlightCts = new CancellationTokenSource();
            HighlightLoopAsync(_sameItemGlow, _highlightCts.Token).Forget();
        }

        private void OnEndHighlight()
        {
            _highlightCts?.Cancel();
            _highlightCts = null;
        }

        private void PlayGlow(GlowPreset preset)
        {
            GlowOnceAsync(preset).Forget();
        }

        private async UniTaskVoid GlowOnceAsync(GlowPreset preset)
        {
            _icon.material = _glowMaterial;
            SetGlowColor(preset.TargetEmissionColor);

            float time = 0f;
            float duration = preset.TargetTime;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(time / duration);

                SetGlowValue(preset.Curve.Evaluate(t));
                transform.localScale = Vector3.one * (1f + preset.ScaleCurve.Evaluate(t));

                await UniTask.Yield(PlayerLoopTiming.Update);
            }

            transform.localScale = Vector3.one;
            _icon.material = null;
        }

        private async UniTaskVoid HighlightLoopAsync(GlowPreset preset, CancellationToken token)
        {
            float time = 0f;
            transform.localScale = Vector3.one;
            _icon.material = _glowMaterial;
            SetGlowColor(preset.TargetEmissionColor);
            ToggleGoldOutline(true);

            try
            {
                while (!token.IsCancellationRequested)
                {
                    time += Time.unscaledDeltaTime;
                    float t = Mathf.Clamp01(time / preset.TargetTime);

                    SetGlowValue(preset.Curve.Evaluate(t));
                    transform.localScale = Vector3.one * (1f + preset.ScaleCurve.Evaluate(t));

                    if (t >= 1f)
                        time = 0f;

                    await UniTask.Yield(PlayerLoopTiming.Update);
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                if (this != null)
                {
                    transform.localScale = Vector3.one;
                    ToggleGoldOutline(false);
                    _icon.material = null;
                }
            }
        }

        private void SetGlowValue(float value) => _icon.material.SetFloat(EmissionValueID, value);
        private void SetGlowColor(Color color) => _glowMaterial.SetColor(GlowColorID, color);

        private void ToggleGoldOutline(bool isGold)
        {
            _outlineImage.gameObject.SetActive(!isGold);
            _goldOutlineImage.gameObject.SetActive(isGold);
        }

        private void SpawnParticles()
        {
            UIParticle particle = Instantiate(_particlePrefab, transform.position, Quaternion.identity, transform);
            float sizeFactor = (GetComponent<RectTransform>().sizeDelta.x + GetComponent<RectTransform>().sizeDelta.y) / InventorySize.CellSize;
            particle.scale = Mathf.Min(5f * sizeFactor, 20f);
        }
    }
}