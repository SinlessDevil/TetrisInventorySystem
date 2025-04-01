using UnityEngine;

namespace Code.InventoryModel.Items.Data
{
    public abstract class WeaponData : ScriptableObject
    {
        [field: SerializeField] public string Id { get; private set; }
    }
}