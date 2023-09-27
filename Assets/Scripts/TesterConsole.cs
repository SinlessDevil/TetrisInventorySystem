using UnityEngine;
using InventorySystem;
using InventorySystem.Abstract;
using InventorySystem.Items;

public class TesterConsole : MonoBehaviour
{
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
        var apple = new Apple(5);
        apple.Amount = randCount;

        _inventory.TryToAdd(this, apple);
    }
    private void RemoveRandApples()
    {
        var randCount = Random.Range(1, 10);
        _inventory.Remove(this, typeof(Apple), randCount);
    }
}