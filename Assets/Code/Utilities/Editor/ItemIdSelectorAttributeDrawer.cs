using System.Linq;
using Code.InventoryModel.Items.Data;
using Code.Utilities.Attributes;
using JetBrains.Annotations;

namespace Utilities.Editor
{
    [UsedImplicitly]
    public class ItemIdSelectorAttributeDrawer : IdSelectorAttributeDrawer<ItemIdSelectorAttribute, ItemData>
    {
        protected override string[] GetIds(ItemData[] data)
        {
            return data.Select(x => x.Id).ToArray();
        }

        protected override string GetPropertyName() => "Item Id";
    }
}