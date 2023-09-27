using InventorySystem.Model;
using UnityEngine;

namespace InventorySystem.Controller
{
    public class InventoryController : MonoBehaviour
    {
        public InventoryWithSlots Inventory { get; private set; }

        private void Awake()
        {
            Inventory = new InventoryWithSlots(20);
        }
    }
}