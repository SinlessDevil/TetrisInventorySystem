using JetBrains.Annotations;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Slot
{
    public class InventorySlotGlowEffect : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [UsedImplicitly]
        private void Deactivate() => Destroy(gameObject);
    }
}