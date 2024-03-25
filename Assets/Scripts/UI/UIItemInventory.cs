using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public Sprite image, image2;
    public int quantity;
    public string itemName, description;


    private void Awake()
    {
        //Hide();
        inventoryDescription.ResetDescription();
        mouseFollower.Toggle(false);
    }
    public void InitInventoryUI(int inventorySize)
    {
        for(int i = 0; i < inventorySize; i++) 
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

    private void HandleShowItemActions(UIItem item)
    {
    }

    private void HandleEndDrag(UIItem item)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleSwap(UIItem item)
    {
        int index = itemList.IndexOf(item);
        if (index == -1)
        {
            mouseFollower.Toggle(false);
            currentDraggingItemIndex = -1;
            return;
        }

        //hardcode
        itemList[1].SetData(image, quantity);
        itemList[0].SetData(image2, quantity);
    }

    private void HandleBeginDrag(UIItem item)
    {
        int index = itemList.IndexOf(item);
        if (index == -1)
            return;
        else
            currentDraggingItemIndex = index;
        mouseFollower.Toggle(true);
        mouseFollower.SetData(index==0?image:image2, quantity);
    }

    private void HandleItemSelection(UIItem item)
    {
        inventoryDescription.SetDescription(image, itemName, description);
        item.Select();  
    }

    public void Show()
    {
        gameObject.SetActive(true);
        inventoryDescription.ResetDescription();
        itemList[0].SetData(image, quantity);
        itemList[1].SetData(image2, quantity);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
