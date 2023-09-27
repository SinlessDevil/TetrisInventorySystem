using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using InventorySystem.Abstract;

namespace InventorySystem
{
    public class InventoryWithSlots : IInventory
    {
        public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;
        public event Action<object, Type, int> OnInventoryItemRemovedEvent;

        public int Capacity { get; set; }

        public bool IsFull => _slots.All(slot => slot.IsFull);

        private List<IInventorySlot> _slots;

        public InventoryWithSlots(int capacity)
        {
            this.Capacity = capacity;

            _slots = new List<IInventorySlot>(capacity);

            for (int i = 0; i < capacity; i++)
                _slots.Add(new InventorySlot());
        }

        public IInventoryItem GetItem(Type itemType) => _slots.Find(slot => slot.ItemType == itemType).Item;
        public IInventoryItem[] GetAllItems()
        {
            var allItems = new List<IInventoryItem>();
            foreach (var slot in _slots)
            {
                if (!slot.IsEmpty)
                    allItems.Add(slot.Item);
            }
            return allItems.ToArray();
        }
        public IInventoryItem[] GetAllItems(Type itemType)
        {
            var allItems = new List<IInventoryItem>();
            var slotsOfType = _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemType);

            foreach (var slot in slotsOfType)
                    allItems.Add(slot.Item);

            return allItems.ToArray();
        }
        public IInventoryItem[] GetEquippedItems()
        {
            var requiredSlots = _slots.
                FindAll(slot => !slot.IsEmpty && slot.Item.IsEquipped);
            var equippedItems = new List<IInventoryItem>();

            foreach (var slot in requiredSlots)
                    equippedItems.Add(slot.Item);

            return equippedItems.ToArray();
        }
        public IInventorySlot[] GetAllSlots(Type itemType) => _slots.FindAll(slot => !slot.IsEmpty && slot.ItemType == itemType).ToArray();
        public IInventorySlot[] GetAllSlots() => _slots.ToArray();
        public int GetItemAmount(Type itemType)
        {
            var amount = 0;
            var allItemSlots = _slots.
                FindAll(slot => !slot.IsEmpty && slot.ItemType == itemType);

            foreach(var itemSlot in allItemSlots)
                amount += itemSlot.Amount;

            return amount;
        }

        public bool TryToAdd(object sender, IInventoryItem item)
        {
            var slotWithSameItemButNotEmpty = _slots.Find(slot => !slot.IsEmpty
            && slot.ItemType == item.Type && !slot.IsFull);

            if (slotWithSameItemButNotEmpty != null)
                return TryAddToSlot(sender, slotWithSameItemButNotEmpty, item);

            var emptySlot = _slots.Find(slot => slot.IsEmpty);
            if(emptySlot != null)
                return TryAddToSlot(sender, emptySlot, item);

            Debug.Log($"Cannot add item ({item.Type}), amount: {item.Amount}, " +
                $"because there is not place for that.");
            return false;
        }
        public void Remove(object sender, Type itemType, int amount = 1)
        {
            var slotsWithItem = GetAllSlots(itemType);
            if (slotsWithItem.Length == 0)
                return;

            var amountToRemove = amount;
            var countSlots = slotsWithItem.Length;

            for (int i = countSlots - 1; i >= 0; i--)
            {
                var slot = slotsWithItem[i];
                if (slot.Amount >= amountToRemove)
                {
                    slot.Item.Amount -= amountToRemove;

                    if(slot.Amount <= 0)
                    {
                        slot.Clear();
                        Debug.Log($"Item removed from inventory. ItemType : {itemType}, Amount: {amountToRemove}");
                        OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountToRemove);
                    }

                    break;
                }

                var amountRemoved = slot.Amount;
                amountToRemove -= slot.Amount;
                slot.Clear();
                Debug.Log($"Item removed from inventory. ItemType : {itemType}, Amount: {amountRemoved}");
                OnInventoryItemRemovedEvent?.Invoke(sender, itemType, amountRemoved);
            }
        }
        public bool HasItem(Type type, out IInventoryItem item)
        {
            item = GetItem(type);
            return item != null;
        }

        private bool TryAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
        {
            var fits = slot.Amount + item.Amount <= item.MaxItemsInInventorySlot;
            var amountToAdd = fits ? item.Amount : item.MaxItemsInInventorySlot - slot.Amount;
            var amountLeft = item.Amount - amountToAdd;
            var clonedItem = item.Clone();

            clonedItem.Amount = amountToAdd;

            if (slot.IsEmpty)
                slot.SetItem(clonedItem);
            else
                slot.Item.Amount += amountToAdd;

            Debug.Log($"Item added to inventory. ItemType : {item.Type}, Amount: {amountToAdd}");
            OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);

            if (amountLeft <= 0)
                return true;

            item.Amount = amountLeft;
            return TryToAdd(sender, item);
        }
    }
}