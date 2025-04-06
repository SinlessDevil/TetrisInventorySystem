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
        private List<ItemContainer> _itemContainers = new(25);
        private List<SlotContainer> _slotContainers = new(25);
        
        private IItemPositionFinding _itemPositionFinding;
        
        private readonly IInventoryPlayerSetUper _inventory;
        private readonly IInventoryUIFactory _inventoryUIFactory;
        private readonly IInventoryPlayerSetUper _inventoryPlayerSetUper;
        private readonly IItemDataProvider _itemDataProvider;

        public InventoryViewInitializer(
            IInventoryPlayerSetUper inventory,
            IInventoryUIFactory inventoryUIFactory,
            IInventoryPlayerSetUper inventoryPlayerSetUper,
            IItemDataProvider itemDataProvider)
        {
            _inventory = inventory;
            _inventoryUIFactory = inventoryUIFactory;
            _inventoryPlayerSetUper = inventoryPlayerSetUper;
            _itemDataProvider = itemDataProvider;
        }
        
        public bool HasOpenInventory => _inventoryContainer != null;
        
        public void OpenInventory()
        {
            BindPositionFinding();
            
            CreateInventory();
            CreateSlots();
            CreateItems();
            
            InitItemPositionFinding();
            
            InitInventory();
            InitSlots();
            InitItems();
        }
        
        public void CloseInventory()
        {
            
        }

        private void BindPositionFinding()
        {
            _itemPositionFinding = new ItemPositionFinding(InventorySize.CellSize);
        }
        
        private void CreateInventory()
        {
            InventoryView inventoryView = _inventoryUIFactory.CreateInventoryView();
            IInventoryViewModel inventoryViewModel = new Inventory.InventoryViewModel(_inventory.Inventory,
                _itemPositionFinding, _slotContainers, _itemContainers);

            InventoryContainer inventoryContainer = new InventoryContainer()
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
                
                SlotContainer slotContainer = new SlotContainer()
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
                    item, _itemPositionFinding, _itemDataProvider, _inventoryContainer.View.ItemsContainer, 
                    InventorySize.CellSize,_inventoryContainer.View.ItemsContainer.position, Quaternion.identity);
                
                ItemContainer itemContainer = new ItemContainer()
                {
                    View = itemView,
                    ViewModel = itemViewModel
                };
                
                _itemContainers.Add(itemContainer);
            }
        }
        
        private void InitItemPositionFinding()
        {
            float offsetX = ((_inventoryContainer.View.ItemsContainer.rect.width / 2) * -1) + InventorySize.CellSize / 2;
            float offsetY = (_inventoryContainer.View.ItemsContainer.rect.height / 2) - InventorySize.CellSize / 2;
            _itemPositionFinding.Initialize(_slotContainers, _inventoryContainer.View.ItemsContainer, offsetX, offsetY);
        }

        
        private void InitInventory()
        {
            _inventoryContainer.View.Initialize();
            _inventoryContainer.ViewModel.Subscribe();
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