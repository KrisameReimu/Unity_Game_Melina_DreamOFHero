using Inventory.Model;
using Inventory.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO inventoryData;
    [SerializeField]
    private GameObject obtainedItemPromptPrefab;
    [SerializeField]
    private Transform collectedObjectList;


    private void Awake()
    {
        if(collectedObjectList == null)
            collectedObjectList = GameObject.Find("UI").transform.Find("CollectedObjectList");
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Item")
            return;

        Item item = collision.gameObject.GetComponent<Item>();
        if(item != null )
        {
            int reminder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
            int obtainedQuantity = item.Quantity - reminder;

            if (reminder == 0)
                item.DestroyItem();
            else
                item.Quantity = reminder;

            ShowObtainedPrompt(item.InventoryItem.itemImage, item.InventoryItem.itemName, obtainedQuantity);
        }
    }

    private void ShowObtainedPrompt(Sprite image, string name, int quantity)
    {
        ObtainedItemPrompt obtainedItem = Instantiate(obtainedItemPromptPrefab, collectedObjectList).GetComponent<ObtainedItemPrompt>();
        obtainedItem.SetData(image, name, quantity);
    }
}
