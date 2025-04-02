using Code.Infrastructure.Factory;
using Zenject;

namespace Code.UI.InventoryViewModel.Factory
{
    public class InventoryUIFactory : IInventoryUIFactory
    {
        private const string InventoryViewPath = "UI/Inventory/InventoryWindow";
        
        private readonly IUIFactory _uiFactory;
        private readonly IInstantiator _instantiator;

        public InventoryUIFactory(IUIFactory uiFactory, IInstantiator instantiator)
        {
            _uiFactory = uiFactory;
            _instantiator = instantiator;
        }
        
        public InventoryView CreateInventoryView()
        {
            var inventoryView = _instantiator.InstantiatePrefabResource(InventoryViewPath, _uiFactory.UIRootCanvas.transform);
            var inventoryViewComponent = inventoryView.GetComponent<InventoryView>();
            if (inventoryViewComponent == null)
            {
                throw new System.Exception("InventoryView component not found on the prefab.");
            }
            
            return inventoryViewComponent;
        }
    }
}