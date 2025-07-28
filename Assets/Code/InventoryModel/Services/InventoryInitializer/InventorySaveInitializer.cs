using System;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.PersistenceProgress.Player;
using Code.Inventory.Services.InventoryExpand;
using Code.InventoryModel;
using Code.InventoryModel.Data;
using Code.InventoryModel.Items.Data;
using Code.InventoryModel.Items.Factory;

namespace Services.Factories.Inventory
{
    public class InventorySaveInitializer : IInventorySaveInitializer
    {
        private readonly IItemFactory _itemFactory;
        private readonly IPersistenceProgressService _progressService;
        private readonly IInventoryExpandService _expandService;

        public InventorySaveInitializer(
            IItemFactory itemFactory,
            IPersistenceProgressService progressService,
            IInventoryExpandService expandService)
        {
            _itemFactory = itemFactory;
            _progressService = progressService;
            _expandService = expandService;
        }

        public InventoryData InventoryData => _progressService.PlayerData.InventoryData;

        public void Initialize(InventoryBalance.ItemId[] initialItems, InventoryBorders defaultBorders)
        {
           // CleanupInventory();
            OpenDefaultCells(defaultBorders);
            PlaceInitialItems(initialItems);
        }

        public void OpenDefaultCells(InventoryBorders defaultBorders)
        {
            foreach (int availableIndex in defaultBorders.GetAvailableIndexes()) 
                InventoryData.InventoryOpening.SetBought(availableIndex);
        }

        public void CleanupInventory()
        {
            InventoryData.PlayerInventory.Items.Clear();

            var cells = InventoryData.PlayerInventory.Cells;
            for (var i = 0; i < cells.Count; i++) 
                cells[i].Clear();
            
            InventoryData.InventoryOpening.BoughtIndexes.Clear();
        }

        public void PlaceInitialItems(InventoryBalance.ItemId[] initialItems)
        {
            var inventory = new ExpandableInventory(new TetrisInventory(InventoryData.PlayerInventory), _expandService);
            for (int i = 0; i < initialItems.Length; i++)
            {
                string itemId = initialItems[i].Id;
                Item item = _itemFactory.Create(itemId);
                bool isAdded = inventory.TryAdd(item);

                if (!isAdded)
                    throw new InvalidOperationException($"Can't add initial item {itemId}");
            }
        }
    }
}