using System.Collections.Generic;
using Code.Inventory.Items.Data;

namespace Code.Inventory
{
    public interface IInventoryAddCondition
    {
        bool CanPlace(Item item, int targetIndex);
        bool IsValid(List<GridCell> willPlaced);
    }
}