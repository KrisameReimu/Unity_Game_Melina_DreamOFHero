using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIItemInventory inventoryUI;

    [SerializeField] 
    private InventorySO inventoryData;

    private void Awake()
    {
        inventoryUI = GameObject.Find("UI").GetComponentInChildren<UIItemInventory>();
        PrepareUI();
        //inventoryData.Initialize();
        inventoryUI.Hide();
    }

    private void PrepareUI()
    {
        inventoryUI.InitInventoryUI(inventoryData.size);
        this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventoryUI.OnSwapItems += HandleSwapItems;
        this.inventoryUI.OnStartDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;
    }

    private void HandleDragging(int itemIndex)
    {
        throw new NotImplementedException();
    }

    private void HandleSwapItems(int currentIndex, int index)
    {
        throw new NotImplementedException();
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }
        Item item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, 
            item.itemImage, item.itemName, item.description);
    }

    private void HandleItemActionRequest(int itemIndex)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            if (!inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState())
                {
                    inventoryUI.UpdateData(item.Key,
                        item.Value.item.itemImage,
                        item.Value.quantity);
                }
            }
               
            else 
                inventoryUI.Hide();
        }
    }

    /*
    IEnumerator SetInventoryUI() 
    {
        inventoryUI = GameObject.Find("Inventory").GetComponent<UIItemInventory>();
        yield return new WaitForSeconds(2);
        Debug.Log("Finish");
        inventoryUI.InitInventoryUI(inventorySize);
    }
    */
}
