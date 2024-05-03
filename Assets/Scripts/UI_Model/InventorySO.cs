using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "ItemSO/InventorySO")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems;
        [field: SerializeField]
        public int size { get; private set; } = 40;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;


        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int GetInventoryIndex(InventoryItem item)
        {
            int index = inventoryItems.IndexOf(item);
            return index;
        }

        public InventoryItem GetItemAt(int index)
        {
            return inventoryItems[index];
        }

        public int AddItem(ItemSO newItem, int newQuantity, List<ItemParameter> itemState = null)
        {
            if (newItem.isStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(newQuantity>0 && !IsInventoryFull())
                    {
                        newQuantity -= AddItemToFirstFreeSlot(newItem, 1, itemState);
                    }
                    InformAboutChange();
                    return newQuantity;
                }
            }
            newQuantity = AddStackableItem(newItem, newQuantity);
            InformAboutChange();
            return newQuantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO theItem, int theQuantity
            , List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = theItem,
                quantity = theQuantity,
                itemState = 
                new List<ItemParameter>(itemState==null?theItem.defaultParametersList:itemState)
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return theQuantity;
                }
            }
            return 0;
        }

        private bool IsInventoryFull()
            => inventoryItems.Where(item => item.IsEmpty).Any() == false;

        private int AddStackableItem(ItemSO newItem, int newQuantity)
        {
            for (int i = 0; i< inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                if (inventoryItems[i].item.ID == newItem.ID)
                {
                    int amountPossibleToTake =
                        inventoryItems[i].item.maxStackSize - inventoryItems[i].quantity;

                    if(newQuantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.maxStackSize);
                        newQuantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + newQuantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while(newQuantity > 0 && IsInventoryFull() == false)
            {
                int remainQuantity = Mathf.Clamp(newQuantity, 0, newItem.maxStackSize);
                newQuantity -= remainQuantity;
                AddItemToFirstFreeSlot(newItem, remainQuantity);
            }
            return newQuantity;
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnValue =
                new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;
                returnValue[i] = inventoryItems[i];
            }
            return returnValue;
        }

        public void SwapItems(int currentIndex, int index)
        {
            InventoryItem item1 = inventoryItems[currentIndex];
            inventoryItems[currentIndex] = inventoryItems[index];
            inventoryItems[index] = item1;
            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public void RemoveItem(int itemIndex, int amount)//call when consume
        {
            if(inventoryItems.Count > itemIndex)//item exists
            {
                if (inventoryItems[itemIndex].IsEmpty)
                    return;
                int remainNum = inventoryItems[itemIndex].quantity - amount;
                if(remainNum <= 0)
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                else
                    inventoryItems[itemIndex] = inventoryItems[itemIndex]
                        .ChangeQuantity(remainNum);

                InformAboutChange();
            }
        }
    }

    [Serializable]
    public struct InventoryItem
    {
        public ItemSO item;
        public int quantity;
        public List<ItemParameter> itemState;
        public bool IsEmpty => item == null;

        public InventoryItem ChangeQuantity(int newQuantity)
        {
            return new InventoryItem
            {
                item = this.item,
                quantity = newQuantity,
                itemState = new List<ItemParameter>(this.itemState),
            };
        }

        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>(),
            };
        }

    }
}
