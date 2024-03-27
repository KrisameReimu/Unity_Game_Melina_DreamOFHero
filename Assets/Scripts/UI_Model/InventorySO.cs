using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemSO/Inventory")]
public class InventorySO : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> inventoryItems;
    [field: SerializeField]
    public int size { get; private set; } = 10;


    public void Initialize() 
    {
        inventoryItems = new List<InventoryItem>();
        for(int i = 0; i < size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public InventoryItem GetItemAt(int index) 
    {
        return inventoryItems[index];
    }

    public void AddItem(Item newIitem, int newQuantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty) 
            {
                inventoryItems[i] = new InventoryItem
                {
                    item = newIitem,
                    quantity = newQuantity
                };
            }
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState() 
    {
        Dictionary<int, InventoryItem> returnValue =
            new Dictionary<int, InventoryItem>();
        for (int i = 0;i < inventoryItems.Count;i++) 
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            returnValue[i] = inventoryItems[i];
        }
        return returnValue; 
    }
}

[Serializable]
public struct InventoryItem
{
    public Item item;
    public int quantity;
    public bool IsEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity,
        };
    }

    public static InventoryItem GetEmptyItem() 
    {
        return new InventoryItem
        {
            item = null,
            quantity = 0,
        };
    }

}