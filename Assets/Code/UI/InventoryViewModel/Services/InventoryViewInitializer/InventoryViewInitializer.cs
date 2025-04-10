using System.Collections.Generic;
using UnityEngine;
using UI.Inventory;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel;
using Code.InventoryModel.Items.Provider;
using Code.InventoryModel.Services.InventoryPlayer;
using Code.UI.InventoryViewModel.Factory;
using Code.UI.InventoryViewModel.Inventory;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Slot;

namespace Code.UI.InventoryViewModel.Services.InventoryViewInitializer
{
    public class InventoryViewInitializer : IInventoryViewInitializer
    {
        private InventoryContainer _inventoryContainer;
        
        private IItemPositionFinding _itemPositionFinding;
        
        private readonly IInventoryPlayerSetUper _inventory;
        private readonly IInventoryUIFactory _inventoryUIFactory;
        private readonly IInventoryPlayerSetUper _inventoryPlayerSetUper;
        private readonly IItemDataProvider _itemDataProvider;
        private readonly IInventoryExpandService _inventoryExpandService;
        private readonly IPersistenceProgressService _persistenceProgressService;
        private readonly IItemDropService _itemDropService;

        public InventoryViewInitializer(
            IInventoryPlayerSetUper inventory,
            IInventoryUIFactory inventoryUIFactory,
            IInventoryPlayerSetUper inventoryPlayerSetUper,
            IItemDataProvider itemDataProvider,
            IInventoryExpandService inventoryExpandService,
            IPersistenceProgressService persistenceProgressService,
            IItemDropService itemDropService)
        {
            _inventory = inventory;
            _inventoryUIFactory = inventoryUIFactory;
            _inventoryPlayerSetUper = inventoryPlayerSetUper;
            _itemDataProvider = itemDataProvider;
            _inventoryExpandService = inventoryExpandService;
            _persistenceProgressService = persistenceProgressService;
            _itemDropService = itemDropService;
        }
        
        public bool HasOpenInventory => _inventoryContainer != null;
        
        public void OpenInventory()
        {
            BindPositionFinding();
            
            CreateInventory();
            var slotContainers = CreateSlots();
            var itemContainers = CreateItems();
            
            InitItemPositionFinding(slotContainers);

            InitializeDropService();
            
            InitInventory(slotContainers, itemContainers);
            InitSlots(slotContainers);
            InitItems(itemContainers);
            
            _inventoryContainer.View.PlayAnimationShow();
        }
        
        public void CloseInventory()
        {
            _inventoryContainer.ViewModel.Unsubscribe();
            
            _inventoryContainer.ViewModel.DisposeViewModel();
            _inventoryContainer.View.Dispose();
            
            _inventoryContainer = null;
            
            _itemDropService.Dispose();
        }

        private void BindPositionFinding()
        {
            _itemPositionFinding = new ItemPositionFinding(InventorySize.CellSize);
        }
        
        private void InitializeDropService()
        {
            _itemDropService.Initialize(_inventoryContainer, _itemPositionFinding);
        }
        
        private void CreateInventory()
        {
            InventoryView inventoryView = _inventoryUIFactory.CreateInventoryView();
            IInventoryViewModel inventoryViewModel = new Inventory.InventoryViewModel(_inventory.Inventory,
                _itemPositionFinding, _itemDropService);

            InventoryContainer inventoryContainer = new InventoryContainer()
            {
                View = inventoryView,
                ViewModel = inventoryViewModel
            };
            
            _inventoryContainer = inventoryContainer;
        }
        
        private List<SlotContainer> CreateSlots()
        {
            List<SlotContainer> slotContainers = new List<SlotContainer>(25);
            
            foreach (GridCell gridCell in _inventoryPlayerSetUper.Inventory.Cells)
            {
                SlotView slotView = _inventoryUIFactory.CreateSlotView(_inventoryContainer.View.SlotsContainer);
                ISlotViewModel slotViewModel = new SlotViewModel(gridCell, _inventoryPlayerSetUper.Inventory, 
                    _inventoryExpandService, _persistenceProgressService);
                
                SlotContainer slotContainer = new SlotContainer()
                {
                    View = slotView,
                    ViewModel = slotViewModel
                };
                
                slotContainers.Add(slotContainer);
            }

            return slotContainers;
        }
        
        private List<ItemContainer> CreateItems()
        {
            List<ItemContainer> itemContainers = new List<ItemContainer>(25);
            
            foreach (InventoryModel.Items.Data.Item item in _inventoryPlayerSetUper.Inventory.Items)
            {
                ItemView itemView = _inventoryUIFactory.CreateItemView(_inventoryContainer.View.ItemsContainer);
                IItemViewModel itemViewModel = new ItemViewModel(item, _itemPositionFinding, _itemDataProvider,
                    _inventoryContainer.View.ItemsContainer, _inventoryContainer.View.ItemDragContainer,
                    InventorySize.CellSize,_inventoryContainer.View.ItemsContainer.position, Quaternion.identity);
                
                ItemContainer itemContainer = new ItemContainer()
                {
                    View = itemView,
                    ViewModel = itemViewModel
                };
                
                itemContainers.Add(itemContainer);
            }

            return itemContainers;
        }
        
        private void InitItemPositionFinding(List<SlotContainer> slotContainers)
        {
            float offsetX = ((_inventoryContainer.View.ItemsContainer.rect.width / 2) * -1) + InventorySize.CellSize / 2;
            float offsetY = (_inventoryContainer.View.ItemsContainer.rect.height / 2) - InventorySize.CellSize / 2;
            _itemPositionFinding.Initialize(slotContainers, 
                _inventoryContainer.View.ItemsContainer, 
                _inventoryContainer.View.DestroyItemContainer,
                _inventoryContainer.View.FreeAreaItemContainer,
                offsetX, offsetY);
        }
        
        private void InitInventory(List<SlotContainer> slotContainers, List<ItemContainer> itemContainers)
        {
            _inventoryContainer.View.Initialize(_inventoryContainer.ViewModel);
            _inventoryContainer.ViewModel.InitializeViewModel(slotContainers, itemContainers);
            _inventoryContainer.ViewModel.Subscribe();
        }
        
        private void InitSlots(List<SlotContainer> slotContainers)
        {
            slotContainers.ForEach(slotContainer =>
            {
                slotContainer.View.Initialize(slotContainer.ViewModel);
                slotContainer.ViewModel.Subscribe();
            });
        }

        private void InitItems(List<ItemContainer> itemContainers)
        {
            itemContainers.ForEach(itemContainer => {itemContainer.View.Initialize(itemContainer.ViewModel);});
        }
    }
}