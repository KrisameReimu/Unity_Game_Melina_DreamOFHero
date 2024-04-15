using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIItemInventory : MonoBehaviour
    {
        [SerializeField]
        private UIItem itemPrefab;

        [SerializeField]
        private RectTransform contentPanel;

        List<UIItem> itemList = new List<UIItem>();

        [SerializeField]
        private UIInventoryDescription inventoryDescription;

        [SerializeField]
        MouseFollower mouseFollower;

        private int currentDraggingItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
                OnItemActionRequested,
                OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            //Hide();
            inventoryDescription.ResetDescription();
            mouseFollower.Toggle(false);
        }
        public void InitInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIItem uiItem = Instantiate(itemPrefab, Vector2.zero, Quaternion.identity);
                uiItem.transform.SetParent(contentPanel); //add to the UI
                itemList.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }


        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (itemList.Count > itemIndex)
            {
                itemList[itemIndex].SetData(itemImage, itemQuantity);
            }
        }
        private void HandleShowItemActions(UIItem item)
        {
            int index = itemList.IndexOf(item);
            if(index == -1)
            {
                return;
            }
        }

        private void HandleEndDrag(UIItem item)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(UIItem item)
        {
            int index = itemList.IndexOf(item);
            if (index == -1)
            {
                return;
            }

            OnSwapItems?.Invoke(currentDraggingItemIndex, index);
            HandleItemSelection(item);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentDraggingItemIndex = -1;
        }

        private void HandleBeginDrag(UIItem item)
        {
            int index = itemList.IndexOf(item);
            if (index == -1)
                return;

            currentDraggingItemIndex = index;
            HandleItemSelection(item);
            OnStartDragging?.Invoke(currentDraggingItemIndex);
        }

        public void CreateDragedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIItem item)
        {
            int index = itemList.IndexOf(item);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }


        public void ResetSelection()
        {
            inventoryDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (UIItem item in itemList)
            {
                item.Deselect();
            }
        }
        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string itemName, string description)
        {
            inventoryDescription.SetDescription(itemImage, itemName, description);
            DeselectAllItems();
            itemList[itemIndex].Select();
        }

        internal void ResetAllItems()
        {
            foreach(var item in itemList)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}