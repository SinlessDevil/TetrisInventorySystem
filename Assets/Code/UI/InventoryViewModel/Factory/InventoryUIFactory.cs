using Code.Infrastructure.Factory;
using Code.UI.InventoryViewModel.Inventory;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Slot;
using UnityEngine;
using Zenject;

namespace Code.UI.InventoryViewModel.Factory
{
    public class InventoryUIFactory : Code.Infrastructure.Factory.Factory, IInventoryUIFactory
    {
        private const string InventoryViewPath = "UI/Inventory/InventoryWindow";
        private const string SlotViewPath = "UI/Inventory/SlotView";
        private const string ItemViewPath = "UI/Inventory/ItemView";
        
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
        
        public SlotView CreateSlotView(RectTransform container)
        {
            var slotView = Instantiate(SlotViewPath, container);
            var slotViewComponent = slotView.GetComponent<SlotView>();
            return slotViewComponent;
        }

        public ItemView CreateItemView(RectTransform container)
        {
            var itemView = Instantiate(ItemViewPath, container);
            var itemViewComponent = itemView.GetComponent<ItemView>();
            return itemViewComponent;
        }
    }
}