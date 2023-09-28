using UnityEngine;
using InventorySystem.Abstract;
using InventorySystem.Model;
using InventorySystem.Model.Items;
using InventorySystem.Model.Items.Types;
using InventorySystem.Model.Items.StaticData;

namespace InventorySystem.Controller
{
    public class TesterConsole : MonoBehaviour
    {
        [SerializeField] private InventoryItemData _info;
        private IInventory _inventory;

        private void Awake()
        {
            var inventoryCapacity = 10;
            _inventory = new InventoryWithSlots(inventoryCapacity);
            Debug.Log($"Inventory initialized, capacity: {inventoryCapacity}");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                AddRandApples();

            if (Input.GetKeyDown(KeyCode.E))
                RemoveRandApples();
        }

        private void AddRandApples()
        {
            var randCount = Random.Range(1, 5);
            var apple = new Apple(_info);
             apple.State.Amount = randCount;

            _inventory.TryToAdd(this, apple);
        }
        private void RemoveRandApples()
        {
            var randCount = Random.Range(1, 10);
            _inventory.Remove(this, TypeItem.Apple, randCount);
        }
    }
}