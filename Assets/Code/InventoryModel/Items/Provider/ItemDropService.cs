using System.Collections.Generic;
using UnityEngine;
using Code.InventoryModel.Items.Data;
using Code.InventoryModel.Items.Factory;
using Code.UI.InventoryViewModel.Factory;
using Code.UI.InventoryViewModel.Item;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using UI.Inventory;

namespace Code.InventoryModel.Items.Provider
{
    public class ItemDropService : IItemDropService
    {
        private InventoryContainer _inventoryContainer;
        private IItemPositionFinding _itemPositionFinding;
        
        private readonly IItemDataProvider _itemDataProvider;
        private readonly IItemFactory _itemFactory;
        private readonly IInventoryUIFactory _inventoryUIFactory;

        public ItemDropService(
            IItemDataProvider itemDataProvider, 
            IItemFactory itemFactory)
        {
            _itemDataProvider = itemDataProvider;
            _itemFactory = itemFactory;
        }

        public void Initialize(
            InventoryContainer inventoryContainer, 
            IItemPositionFinding itemPositionFinding)
        {
            _inventoryContainer = inventoryContainer;
            _itemPositionFinding = itemPositionFinding;
        }
        
        public void Dispose()
        {
            _inventoryContainer = null;
            _itemPositionFinding = null;
        }
        
        public List<ItemContainer> DropItemContainers()
        {
            List<ItemContainer> itemsContainers = new List<ItemContainer>(ItemDropData.CountItems);
            
            List<string> itemIds = GetItemsId();

            foreach (var itemId in itemIds)
            {
                Item item = _itemFactory.Create(itemId);
                ItemContainer itemContainer = CreateItems(item);
                itemsContainers.Add(itemContainer);
            }

            return itemsContainers;
        }

        public ItemContainer CreateItems(Item item)
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

            return itemContainer;
        }
        
        private List<string> GetItemsId()
        {
            List<string> result = new List<string>();
            List<ItemDrop> drops = ItemDropData.ItemDrops;
            int countToDrop = Mathf.Min(ItemDropData.CountItems, drops.Count);

            for (int i = 0; i < countToDrop; i++)
            {
                var totalWeight = 0;
                foreach (var drop in drops)
                {
                    totalWeight += drop.Weight;
                }

                int randomWeight = Random.Range(0, totalWeight);
                int currentWeight = 0;

                foreach (var drop in drops)
                {
                    currentWeight += drop.Weight;
                    if (randomWeight < currentWeight)
                    {
                        result.Add(drop.ItemId.ToString());
                        break;
                    }
                }
            }
            return result;
        }
        
        private ItemDropData ItemDropData => _itemDataProvider.ItemDropData;
    }
}