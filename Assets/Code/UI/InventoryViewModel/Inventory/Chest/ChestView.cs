using UnityEngine;

namespace Code.UI.InventoryViewModel.Inventory.Chest
{
    public class ChestView : MonoBehaviour
    {
        private const string ChestOpenTrigger = "IsOpen";
        private const string ChestCloseTrigger = "IsClose";
        
        [SerializeField] private Animator _animator;

        public bool IsOpen { get; private set; } = false;
        
        public void OpenChest()
        {
            _animator.SetTrigger(ChestOpenTrigger);
        }

        public void CloseChest()
        {
            _animator.SetTrigger(ChestCloseTrigger);
        }
        
        private void OnChestOpened()
        {
            IsOpen = true;
        }

        private void OnChestClosed()
        {
            Destroy(this.gameObject);
        }
    }
}