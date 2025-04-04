using System;
using System.Collections.Generic;
using Code.InventoryModel;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using UnityEngine;

namespace UI.Inventory
{
    public interface IItemPositionFinding
    {
        bool TryGetPositionItemById(Guid itemId);
        bool TryToPlaceItemInInventory(Vector2 position);

        ItemContainer GetNeighbourItemDataWithoutInventory(
            List<ItemContainer> itemsData, ItemContainer targetItemsData);

        Vector2 GetPositionItemInSlotById(Guid itemId);
        Vector2 GetPositionItemInContainer(Vector2 itemSize, Vector2 offset);
        GridCell GetNeighbourGritCellByPosition(Vector2 position);
        Vector2 GetRootPositionByRootIndex(float rootPositionX, float rootPositionY);
        bool TryToPlaceItemInContainer(RectTransform container, Vector2 position);
    }
}