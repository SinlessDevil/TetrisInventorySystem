using System.Collections.Generic;
using Code.InventoryModel.Items.Data;

namespace Code.InventoryModel.InventoryAddCondition
{
    public interface IInventoryAddCondition
    {
        bool CanPlace(Item item, int targetIndex);
        bool IsValid(List<GridCell> willPlaced);
    }
}