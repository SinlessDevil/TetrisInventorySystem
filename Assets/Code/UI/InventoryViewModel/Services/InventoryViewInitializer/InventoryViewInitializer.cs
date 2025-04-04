using Code.UI.InventoryViewModel.Factory;
using Code.UI.InventoryViewModel.Inventory;
using UnityEngine.Rendering;

namespace Code.UI.InventoryViewModel.Services.InventoryViewInitializer
{
    public class InventoryViewInitializer : IInventoryViewInitializer
    {
        private InventoryContainer _inventoryContainer;
        
        private readonly IInventoryUIFactory _inventoryUIFactory;

        public InventoryViewInitializer(IInventoryUIFactory inventoryUIFactory)
        {
            _inventoryUIFactory = inventoryUIFactory;
        }
        
        public bool HasOpenInventory => _inventoryContainer != null;
        
        public void OpenInventory()
        {
            InitializeInventory();
        }

        public void CloseInventory()
        {
            
        }

        private void InitializeInventory()
        {
            InventoryView inventoryView = _inventoryUIFactory.CreateInventoryView();
            IInventoryViewModel inventoryViewModel = new Inventory.InventoryViewModel();

            var inventoryContainer = new InventoryContainer()
            {
                View = inventoryView,
                ViewModel = inventoryViewModel
            };
            
            _inventoryContainer = inventoryContainer;
        }
    }
}