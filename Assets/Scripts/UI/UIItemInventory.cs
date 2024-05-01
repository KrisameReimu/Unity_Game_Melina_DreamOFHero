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

        List<UIItem> UIItemList = new List<UIItem>();

        [SerializeField]
        private UIInventoryDescription inventoryDescription;

        [SerializeField]
        MouseFollower mouseFollower;

        private int currentDraggingItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
                OnItemActionRequested,
                OnStartDragging;

        public event Action<int, int> OnSwapItems;

        [SerializeField]
        private ItemActionPanel actionPanel;
        [SerializeField]
        private UICardSlotPopUp cardSlotPopUp;


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
                UIItemList.Add(uiItem);
                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }


        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (UIItemList.Count > itemIndex)
            {
                UIItemList[itemIndex].SetData(itemImage, itemQuantity);
            }
        }
        private void HandleShowItemActions(UIItem item)
        {
            int index = UIItemList.IndexOf(item);
            if(index == -1)
            {
                return;
            }
            HandleItemSelection(item);
            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UIItem item)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(UIItem item)
        {
            int index = UIItemList.IndexOf(item);
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
            int index = UIItemList.IndexOf(item);
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
            int index = UIItemList.IndexOf(item);
            if (index == -1)
                return;
            OnDescriptionRequested?.Invoke(index);
        }


        public void ResetSelection()
        {
            inventoryDescription.ResetDescription();
            DeselectAllItems();
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction);
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = UIItemList[itemIndex].transform.position;
        }

        public void HideItemAction()
        {
            actionPanel.Toggle(false);
        }

        

        private void DeselectAllItems()
        {
            foreach (UIItem item in UIItemList)
            {
                item.Deselect();
            }
            inventoryDescription.RemoveOldDescriptionButtons();//reset
            actionPanel.Toggle(false);//reset
        }
        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            cardSlotPopUp.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite itemImage, string itemName, string description)
        {
            inventoryDescription.SetDescription(itemImage, itemName, description);
            DeselectAllItems();
            UIItemList[itemIndex].Select();
        }

        public void AddDescriptionBtn(string actionName, Action performAction)
        {
            inventoryDescription.AddDescriptionButton(actionName, performAction);
        }

        internal void ResetAllItems()
        {
            foreach(var item in UIItemList)
            {
                item.ResetData();
                item.Deselect();
            }
        }
    }
}