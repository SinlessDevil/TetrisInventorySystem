using System;
using System.Collections;
using Code.InventoryModel;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace Code.UI.InventoryViewModel.Item
{
    public class ItemEffecter : MonoBehaviour
    {
        private static readonly int EmissionValueID = Shader.PropertyToID("_Emission");
        private static readonly int GlowColorID = Shader.PropertyToID("_GlowColor");

        [Space(10)] [Header("Main")]
        [SerializeField] private Image _outlineImage;
        [SerializeField] private Image _goldOutlineImage;
        [Space(10)] [Header("Additional")]
        [SerializeField] private UIParticle _particleSystem;
        [Space(10)] [Header("GlowPresets")]
        [SerializeField] private GlowPreset _itemMergeGlowPreset;
        [SerializeField] private GlowPreset _itemDropGlowPreset;
        [SerializeField] private GlowPreset _sameItemGlowPreset;
        
        private Coroutine _playGlowCoroutine;
        
        private Material _glowMaterial;
        private bool _sameItemsEffectEnabled;
        
        private IItemViewModel _itemVM;
        private Image _icon;

        public void Initialize(
            IItemViewModel itemVM,
            Image icon)
        {
            _icon = icon;
            _itemVM = itemVM;
            
            _glowMaterial = Instantiate(_icon.material);
            _glowMaterial.name = _icon.material.name + "_Clone";
            
            SetGlowMaterial(false);
            SetGlowValue(0);
            
            SetActivateGoldOutline(false);
            
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void SetGlowValue(float value) => _icon.material.SetFloat(EmissionValueID, value);

        private void SetGlowMaterial(bool value) => _icon.material = value ? _glowMaterial : null;

        private void SetGlowColor(Color glowColor) => _glowMaterial.SetColor(GlowColorID, glowColor);
        
        private void SetActivateGoldOutline(bool value)
        {
            _outlineImage.gameObject.SetActive(!value);
            _goldOutlineImage.gameObject.SetActive(value);
        }
        
        private void Subscribe()
        {
            _itemVM.EffectDropItemEvent += OnDropItemEffectWrap;
            _itemVM.EffectStackItemEvent += OnMergeItemsWrap;
        }
        private void Unsubscribe()
        {
            _itemVM.EffectDropItemEvent -= OnDropItemEffectWrap;
            _itemVM.EffectStackItemEvent -= OnMergeItemsWrap;
        }

        private void OnDropItemEffectWrap(IItemViewModel itemViewModel)
        {
            StartCoroutine(PlayEffectGlowRoutine(_itemDropGlowPreset));
        }
        
        private void OnItemDragEnded()
        {
            _sameItemsEffectEnabled = false;
        }
        
        private void OnPlayEffectHighlightItemWrap()
        {
            _sameItemsEffectEnabled = true;
            if (_playGlowCoroutine != null)
                StopCoroutine(_playGlowCoroutine);
            
            _playGlowCoroutine = StartCoroutine(PlayEffectHighlightRoutine(_sameItemGlowPreset));
        }
        
        private void OnMergeItemsWrap()
        {
            var particleEffect = Instantiate(_particleSystem, transform);
            particleEffect.transform.position = transform.position;
            var itemSize = this.GetComponent<RectTransform>().sizeDelta;
            var grids = (itemSize.x + itemSize.y) / InventorySize.CellSize;
            particleEffect.scale = Mathf.Min(10 * grids, 30);
            
            StartCoroutine(PlayEffectGlowRoutine(_itemMergeGlowPreset));
        }

        private IEnumerator PlayEffectGlowRoutine(GlowPreset glowPreset)
        {
            var time = 0f;
            SetGlowMaterial(true);
            var startScale = Vector3.one;
            SetGlowColor(glowPreset.TargetEmissionColor);
            while (time < glowPreset.TargetTime)
            {
                if(this == null)
                    yield break;
                
                time += Time.unscaledDeltaTime;
                var value = Mathf.Clamp01(time / glowPreset.TargetTime);
                SetGlowValue(glowPreset.Curve.Evaluate(value));
                var scale = 1f + glowPreset.ScaleCurve.Evaluate(value);
                transform.localScale = Vector3.one * scale;
                yield return typeof(WaitForEndOfFrame);
            }

            transform.localScale = startScale;
            SetGlowMaterial(false);
        }
        
        private IEnumerator PlayEffectHighlightRoutine(GlowPreset glowPreset)
        {
            var time = 0f;
            
            transform.localScale = Vector3.one;
            SetActivateGoldOutline(true);
            SetGlowColor(glowPreset.TargetEmissionColor);
            SetGlowMaterial(true);
            
            while (_sameItemsEffectEnabled)
            {
                time += Time.unscaledDeltaTime;
                var value = Mathf.Clamp01(time / glowPreset.TargetTime);
                var scale = 1f + glowPreset.ScaleCurve.Evaluate(value);
               
                SetGlowValue(glowPreset.Curve.Evaluate(value));
                transform.localScale = Vector3.one * scale;
                
                yield return typeof(WaitForEndOfFrame);
                if (value >= 1)
                    time = 0f;
            }
            
            transform.localScale = Vector3.one;
            SetActivateGoldOutline(false);
            SetGlowMaterial(false);

            _playGlowCoroutine = null;
        }
    }
    
            
    [Serializable]
    public class GlowPreset
    {
        public float TargetTime;
        public AnimationCurve Curve;
        public AnimationCurve ScaleCurve;
        [SerializeField, ColorUsage(true, true)] public Color TargetEmissionColor;
    }
}