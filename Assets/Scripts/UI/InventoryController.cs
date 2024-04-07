using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private UIItemInventory inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initItems = new List<InventoryItem>();

        private void Awake()
        {
            inventoryUI = GameObject.Find("UI").GetComponentInChildren<UIItemInventory>();
            PrepareUI();
            PrepareInventoryData();
            inventoryUI.Hide();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initItems) 
            {
                if(item.IsEmpty) 
                    continue;
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.item.itemImage, item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitInventoryUI(inventoryData.size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;
            inventoryUI.CreateDragedItem(inventoryItem.item.itemImage, inventoryItem.quantity);

        }

        private void HandleSwapItems(int currentIndex, int index)
        {
            inventoryData.SwapItems(currentIndex, index);
        }

        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.item;
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
}