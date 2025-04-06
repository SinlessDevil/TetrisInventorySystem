using UnityEngine;

namespace Code.UI.InventoryViewModel.Slot
{
    public class SlotView : MonoBehaviour
    {
        private ISlotViewModel _viewModel;
        
        public void Initialize(ISlotViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        
    }
}