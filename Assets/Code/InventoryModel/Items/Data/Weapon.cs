using System;
using Code.Utilities.Attributes;

namespace Code.InventoryModel.Items.Data
{
    [Serializable]
    public class Weapon : Item
    {
        [WeaponIdSelector(HasGameObjectField = true)]
        public string WeaponId;
        
        public Weapon()
        {
            InventoryPlacement = new InventoryPlacement(size: 3);
        }
    }
}