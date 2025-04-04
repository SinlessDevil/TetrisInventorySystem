using System.Collections.Generic;
using Code.InventoryModel;
using Code.InventoryModel.Items.Provider;
using Code.InventoryModel.Services.InventoryPlayer;
using Code.UI.InventoryViewModel.Factory;
using Code.UI.InventoryViewModel.Inventory;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Slot;
using UI.Inventory;
using UnityEngine;

namespace Code.UI.InventoryViewModel.Services.InventoryViewInitializer
{
    public class InventoryViewInitializer : IInventoryViewInitializer
    {
        private InventoryContainer _inventoryContainer;
        private List<ItemContainer> _itemContainers = new List<ItemContainer>(25);
        private List<SlotContainer> _slotContainers = new List<SlotContainer>(25);
        
        private IItemPositionFinding _itemPositionFinding;
        
        private readonly IInventoryUIFactory _inventoryUIFactory;
        private readonly IInventoryPlayerSetUper _inventoryPlayerSetUper;
        private readonly IItemDataProvider _itemDataProvider;

        public InventoryViewInitializer(
            IInventoryUIFactory inventoryUIFactory,
            IInventoryPlayerSetUper inventoryPlayerSetUper,
            IItemDataProvider itemDataProvider)
        {
            _inventoryUIFactory = inventoryUIFactory;
            _inventoryPlayerSetUper = inventoryPlayerSetUper;
            _itemDataProvider = itemDataProvider;
        }
        
        public bool HasOpenInventory => _inventoryContainer != null;
        
        public void OpenInventory()
        {
            CreateInventory();
            CreateSlots();
            
            InitPositionFinding();
            CreateItems();
            
            InitInventory();
            InitSlots();
            InitItems();
        }

        public void CloseInventory()
        {
            
        }

        private void CreateInventory()
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
        
        private void CreateSlots()
        {
            foreach (GridCell gridCell in _inventoryPlayerSetUper.Inventory.Cells)
            {
                SlotView slotView = _inventoryUIFactory.CreateSlotView(_inventoryContainer.View.SlotsContainer);
                ISlotViewModel slotViewModel = new SlotViewModel(gridCell);
                
                var slotContainer = new SlotContainer()
                {
                    View = slotView,
                    ViewModel = slotViewModel
                };
                
                _slotContainers.Add(slotContainer);
            }
        }
        
        private void CreateItems()
        {
            foreach (InventoryModel.Items.Data.Item item in _inventoryPlayerSetUper.Inventory.Items)
            {
                ItemView itemView = _inventoryUIFactory.CreateItemView(_inventoryContainer.View.ItemsContainer);
                IItemViewModel itemViewModel = new ItemViewModel(
                    item, _itemPositionFinding, _inventoryContainer.View.ItemsContainer, _itemDataProvider, 
                    InventorySize.CellSize,_inventoryContainer.View.ItemsContainer.position, Quaternion.identity);
                
                var itemContainer = new ItemContainer()
                {
                    View = itemView,
                    ViewModel = itemViewModel
                };
                
                _itemContainers.Add(itemContainer);
            }
        }

        private void InitPositionFinding()
        {
            var offsetX = ((_inventoryContainer.View.ItemsContainer.rect.width / 2) * -1) + InventorySize.CellSize / 2;
            var offsetY = (_inventoryContainer.View.ItemsContainer.rect.height / 2) - InventorySize.CellSize / 2;
            _itemPositionFinding = new ItemPositionFinding(_slotContainers, InventorySize.CellSize, offsetX, offsetY, 
                _inventoryContainer.View.ItemsContainer);
        }

        private void InitInventory()
        {
            _inventoryContainer.View.Initialize();
        }
        
        private void InitSlots()
        {
            _slotContainers.ForEach(slotContainer => {slotContainer.View.Initialize(slotContainer.ViewModel);});
        }

        private void InitItems()
        {
            _itemContainers.ForEach(itemContainer => {itemContainer.View.Initialize(itemContainer.ViewModel);});
        }
    }
}