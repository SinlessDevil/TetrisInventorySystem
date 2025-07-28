using System;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Item
{
    [Serializable]
    public class GlowPreset
    {
        public float TargetTime;
        public AnimationCurve Curve;
        public AnimationCurve ScaleCurve;
        [SerializeField, ColorUsage(true, true)] public Color TargetEmissionColor;
    }
}