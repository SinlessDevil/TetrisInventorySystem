using System.Collections.Generic;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel.Items.Data;

namespace Code.InventoryModel.InventoryAddCondition
{
    public class CellBoughtCondition : IInventoryAddCondition
    {
        private readonly IInventoryExpandService _expandService;

        public CellBoughtCondition(IInventoryExpandService expandService)
        {
            _expandService = expandService;
        }

        public bool CanPlace(Item item, int targetIndex)
        {
            return _expandService.IsOpened(targetIndex);
        }

        public bool IsValid(List<GridCell> willPlaced)
        {
            return true;
        }
    }
}