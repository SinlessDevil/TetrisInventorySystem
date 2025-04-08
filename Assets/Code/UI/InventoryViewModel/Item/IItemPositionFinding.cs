using System;
using System.Collections.Generic;
using UnityEngine;
using Code.InventoryModel;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;

namespace UI.Inventory
{
    public interface IItemPositionFinding
    {
        public void Initialize(List<SlotContainer> slotsData, 
            RectTransform containerInInventory, 
            RectTransform destroyItemContainer,
            RectTransform freeAreaItemContainer,
            float offsetX, float offsetY);
        
        public bool TryToPlaceItemInInventory(Vector2 position);
        public bool TryToPlaceItemInDestroyContainer(Vector2 position);
        public bool TryToPlaceItemFreeAreaContainer(Vector2 currentPosition);
        public bool TryToPlaceItemInContainer(RectTransform container, Vector2 position);
        public bool TryGetPositionItemById(Guid itemId);

        public ItemContainer GetNeighbourItemDataWithoutInventory(List<ItemContainer> itemsData, ItemContainer targetItemsData);
        public Vector2 GetPositionItemInSlotById(Guid itemId);
        public Vector2 GetPositionItemInContainer(Vector2 itemSize, Vector2 offset);
        public GridCell GetNeighbourGritCellByPosition(Vector2 position);
        public Vector2 GetRootPositionByRootIndex(float rootPositionX, float rootPositionY);
    }
}