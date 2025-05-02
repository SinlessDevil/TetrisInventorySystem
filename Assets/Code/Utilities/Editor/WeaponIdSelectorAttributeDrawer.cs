using System.Linq;
using Code.InventoryModel.Items.Data;
using JetBrains.Annotations;
using Utilities.Attributes;

namespace Utilities.Editor
{
    [UsedImplicitly]
    public class WeaponIdSelectorAttributeDrawer : IdSelectorAttributeDrawer<WeaponIdSelectorAttribute, WeaponData>
    {
        protected override string[] GetIds(WeaponData[] data)
        {
            return data.Select(x => x.Id).ToArray();
        }

        protected override string GetPropertyName() => "Weapon Id";
    }
}