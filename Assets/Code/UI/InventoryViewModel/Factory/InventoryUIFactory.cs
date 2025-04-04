using Code.Infrastructure.Factory;
using Code.UI.InventoryViewModel.Inventory;
using Zenject;

namespace Code.UI.InventoryViewModel.Factory
{
    public class InventoryUIFactory : Code.Infrastructure.Factory.Factory, IInventoryUIFactory
    {
        private const string InventoryViewPath = "UI/Inventory/InventoryWindow";
        
        private readonly IUIFactory _uiFactory;
        public InventoryUIFactory(IUIFactory uiFactory, IInstantiator instantiator) : base(instantiator)
        {
            _uiFactory = uiFactory;
        }
        
        public InventoryView CreateInventoryView()
        {
            var inventoryView = Instantiate(InventoryViewPath, _uiFactory.UIRootCanvas.transform);
            var inventoryViewComponent = inventoryView.GetComponent<InventoryView>();
            return inventoryViewComponent;
        }
    }
}