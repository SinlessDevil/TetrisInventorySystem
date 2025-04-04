using Code.InventoryModel;

namespace Code.UI.InventoryViewModel.Slot
{
    public class SlotViewModel : ISlotViewModel
    {
        private readonly GridCell _gridCell;
        
        public SlotViewModel(GridCell gridCell)
        {
            _gridCell = gridCell;
        }
        
        public GridCell GridCell => _gridCell;
    }
}