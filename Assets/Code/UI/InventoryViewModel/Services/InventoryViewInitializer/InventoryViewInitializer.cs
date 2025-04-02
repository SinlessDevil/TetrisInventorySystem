using Code.UI.InventoryViewModel.Factory;

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
            IInventoryViewModel inventoryViewModel = new InventoryViewModel();

            var inventoryContainer = new InventoryContainer()
            {
                View = inventoryView,
                ViewModel = inventoryViewModel
            };
            
            _inventoryContainer = inventoryContainer;
        }
    }
}