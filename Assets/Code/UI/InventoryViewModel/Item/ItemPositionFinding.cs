using System;
using System.Linq;
using System.Collections.Generic;
using Code.InventoryModel;
using Code.UI.InventoryViewModel.Services.InventoryViewInitializer;
using UnityEngine;

namespace UI.Inventory
{
    public class ItemPositionFinding : IItemPositionFinding
    {
        private List<SlotContainer> _slotsData;
        private RectTransform _containerInInventory;
        private int _offsetX;
        private int _offsetY;
        
        private readonly int _cellSize;
        
        public ItemPositionFinding(float cellSize)
        {
            _cellSize = (int)cellSize;
        }

        public void Initialize(            
            List<SlotContainer> slotsData,
            RectTransform containerInInventory,
            float offsetX, float offsetY)
        {
            _slotsData = slotsData;
            _containerInInventory = containerInInventory;
            _offsetX = (int)offsetX;
            _offsetY = (int)offsetY;
        }
        
        public bool TryGetPositionItemById(Guid itemId)
        {
            List<SlotContainer> slotsDataWithItem = GetSlotsDataWithItem(itemId);

            if (slotsDataWithItem == null || slotsDataWithItem.Count == 0)
                return false;

            return true;
        }
        
        public bool TryToPlaceItemInInventory(Vector2 position)
        {
            return TryToPlaceItemInContainer(_containerInInventory, position);
        }

        public ItemContainer GetNeighbourItemDataWithoutInventory(
            List<ItemContainer> itemsData, ItemContainer targetItemsData)
        {
            ItemContainer closesData = null;
            float minDistance = float.MaxValue;
            float maxDistance = 5000;
            
            foreach (var itemData in itemsData)
            {
                float distance = ((Vector2)targetItemsData.View.transform.position - 
                                  (Vector2)itemData.View.transform.position).sqrMagnitude;
                if (distance > minDistance)
                    continue;

                minDistance = distance;
                closesData = itemData;
            }

            if (closesData == null)
                return null;
            
            float currentDistanceItemData = ((Vector2)targetItemsData.View.transform.position - 
                                             (Vector2)closesData.View.transform.position).sqrMagnitude;
            if (currentDistanceItemData > maxDistance)
                return null;

            return closesData;
        }
        
        public Vector2 GetPositionItemInSlotById(Guid itemId)
        {
            List<SlotContainer> slotsDataWithItem = GetSlotsDataWithItem(itemId);

            if (slotsDataWithItem == null || slotsDataWithItem.Count == 0)
                return Vector2.zero;

            List<Vector2> positionSlots = GetPositionSlotsByIndex(slotsDataWithItem);
            Vector2 positionItem = GetCenterPositionItem(positionSlots);

            return positionItem;
        }
        
        public Vector2 GetPositionItemInContainer(Vector2 itemSize, Vector2 offset)
        {
            Vector2 position = new Vector2(offset.x * itemSize.x, offset.y * itemSize.y);
            return position;
        }
        
        public GridCell GetNeighbourGritCellByPosition(Vector2 position)
        {
            SlotContainer closesData = null;
            float minDistance = float.MaxValue;
            float maxDistance = 10000f;
            
            foreach (var slotData in _slotsData)
            {
                float distance = (position - (Vector2)slotData.View.transform.position).sqrMagnitude;
                if (distance > minDistance)
                    continue;

                minDistance = distance;
                closesData = slotData;
            }

            if (closesData == null)
                return null;
            
            float currentDistanceSlot = (position - (Vector2)closesData.View.transform.position).sqrMagnitude;
            if (currentDistanceSlot > maxDistance)
                return null;
            
            return closesData?.ViewModel.GridCell;
        }
        
        public Vector2 GetRootPositionByRootIndex(float rootPositionX, float rootPositionY)
        {
            Vector2 position;
            position.x = _cellSize / 2 * rootPositionX;
            position.y = _cellSize / 2 * rootPositionY;
            return position;
        }

        public bool TryToPlaceItemInContainer(RectTransform container, Vector2 position)
        {
            if (container == null || !container.gameObject.activeSelf || container.rect.size == Vector2.zero)
            {
                return false;
            }
            
            Vector2 objectCenter = position;
    
            Vector2 containerCenter = container.position;
            Vector2 containerSize = container.rect.size;
            
            float reductionFactorX = 0.19f;
            float reductionFactorY = 0.20f;
            float reducedSizeX = containerSize.x;
            float reducedSizeY = containerSize.y;
    
            float containerMinX = containerCenter.x - reducedSizeX / 2;
            float containerMaxX = containerCenter.x + reducedSizeX / 2;
            float containerMinY = containerCenter.y - reducedSizeY / 2;
            float containerMaxY = containerCenter.y + reducedSizeY / 2;
    
            bool insideContainer = objectCenter.x >= containerMinX &&
                                   objectCenter.x <= containerMaxX &&
                                   objectCenter.y >= containerMinY &&
                                   objectCenter.y <= containerMaxY;

            return insideContainer;
        }

        private List<SlotContainer> GetSlotsDataWithItem(Guid itemId)
        {
            return _slotsData.Where(slotData => 
                slotData.ViewModel.GridCell.Item != null && 
                slotData.ViewModel.GridCell.Item.InstanceId == itemId).ToList();
        }

        private List<Vector2> GetPositionSlotsByIndex(List<SlotContainer> slotsDataWithItem)
        {
            int positionOffset = (_cellSize / 2);

            return slotsDataWithItem.Select(slotData =>
            {
                int positionXByIndex = _offsetX + positionOffset + (_cellSize * slotData.ViewModel.GridCell.GridX);
                int positionYByIndex = _offsetY - (positionOffset + (_cellSize * slotData.ViewModel.GridCell.GridY));
                return new Vector2(positionXByIndex, positionYByIndex);
            }).ToList();
        }

        private Vector2 GetCenterPositionItem(List<Vector2> positionSlots)
        {
            float totalX = 0f;
            float totalY = 0f;
            foreach (Vector2 position in positionSlots)
            {
                totalX += position.x;
                totalY += position.y;
            }

            float averageX = totalX / positionSlots.Count;
            float averageY = totalY / positionSlots.Count;
            Vector2 averagePosition = new Vector2(averageX, averageY);
            return averagePosition;
        }
    }
}