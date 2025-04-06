using System;
using System.Collections.Generic;
using Code.InventoryModel;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using UnityEngine;

namespace UI.Inventory
{
    public interface IItemPositionFinding
    {
        public void Initialize(List<SlotContainer> slotsData, RectTransform containerInInventory, float offsetX, float offsetY);
        
        public ItemContainer GetNeighbourItemDataWithoutInventory(List<ItemContainer> itemsData, ItemContainer targetItemsData);
        public Vector2 GetPositionItemInSlotById(Guid itemId);
        public Vector2 GetPositionItemInContainer(Vector2 itemSize, Vector2 offset);
        public GridCell GetNeighbourGritCellByPosition(Vector2 position);
        public Vector2 GetRootPositionByRootIndex(float rootPositionX, float rootPositionY);
        
        public bool TryToPlaceItemInContainer(RectTransform container, Vector2 position);
        public bool TryGetPositionItemById(Guid itemId);
        public bool TryToPlaceItemInInventory(Vector2 position);
    }
}