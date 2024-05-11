using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        private static InventoryController inventoryControllerInstance;
        [SerializeField]
        private UIItemInventory inventoryUI;

        [SerializeField]
        private InventorySO inventoryData;

        public List<InventoryItem> initItems = new List<InventoryItem>();

        

        private void Awake()
        {
            if (inventoryControllerInstance == null)
            {
                inventoryControllerInstance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }



            if (inventoryUI != null)
                return;

            //Debug.Log("awake");

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
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex,
                item.itemImage, item.itemName, description);
            //set description button
            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null) // cast successfully
            {
                inventoryUI.AddDescriptionBtn(itemAction.ActionName, () => PerformAction(itemIndex));
            }
        }

        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.description);
            sb.AppendLine();//new line
            for(int i = 0; i < inventoryItem.itemState.Count; i++)
            {
                sb.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} " +
                    $": {inventoryItem.itemState[i].value} ");
                    //$"/ {inventoryItem.item.defaultParametersList[i].value}");
                sb.AppendLine();//new line
            }
            return sb.ToString();
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null) // cast successfully
            {
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(itemIndex));
            }

            inventoryUI.AddAction("Close", () => inventoryUI.HideItemAction());

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null) // cast successfully
            {
                INonDestroyableItem superCard = inventoryItem.item as INonDestroyableItem;
                if (superCard == null)//is not super card
                {
                    inventoryUI.AddAction("Destroy", () => DropItem(itemIndex, inventoryItem.quantity));
                }
            }
        }


        private void DropItem(int itemIndex, int quantity)
        {
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryUI.ResetSelection();
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            if (inventoryItem.IsEmpty)
                return;

            IDestroyableItem destroyableItem = inventoryItem.item as IDestroyableItem;
            if (destroyableItem != null) // cast successfully
            {
                if (!(inventoryItem.item is CardItemSO))//no consume if is Card
                    inventoryData.RemoveItem(itemIndex, 1);//consume
            }

            IItemAction itemAction = inventoryItem.item as IItemAction;
            if (itemAction != null) // cast successfully
            {                            //player
                itemAction.PerformAction(gameObject, inventoryItem);//active effect
                if(inventoryData.GetItemAt(itemIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }
            }

            inventoryUI.HideItemAction();
        }

        //for save & load
        public Dictionary<int, InventoryItem> GetPlayerInventoryContent()
        {
            return inventoryData.GetCurrentInventoryState();
        }

        public void LoadInventoryData(int[,] invenotryDataArray)
        {
            inventoryData.LoadInventoryData(invenotryDataArray);
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

        void OnDestroy()
        {
            if (inventoryControllerInstance != this)
                return;

            inventoryUI.OnDescriptionRequested -= HandleDescriptionRequest;
            inventoryUI.OnSwapItems -= HandleSwapItems;
            inventoryUI.OnStartDragging -= HandleDragging;
            inventoryUI.OnItemActionRequested -= HandleItemActionRequest;

            inventoryData.OnInventoryUpdated -= UpdateInventoryUI;

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