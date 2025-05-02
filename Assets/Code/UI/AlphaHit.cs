using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    [RequireComponent(typeof(Image))]
    public class AlphaHit : MonoBehaviour
    {
        [Range(0, 1)]
        public float alphaThreshold = 0.1f;

        private void Awake()
        {
            var image = GetComponent<Image>();
            image.alphaHitTestMinimumThreshold = alphaThreshold;
        }
    }   
}