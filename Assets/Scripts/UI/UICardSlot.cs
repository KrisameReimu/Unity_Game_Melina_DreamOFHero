using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardSlot : MonoBehaviour
{
    [SerializeField]
    private CardItemSO card;//current equiping card
    [SerializeField]
    private InventorySO inventoryData;
    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;
    [SerializeField]
    private InventoryItem inventoryItem;
    [field: SerializeField]
    private Image cardImage;
    [field: SerializeField]
    private Sprite slotDefaultImage;
    [SerializeField]
    private int currentIndex;

    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }
    public void SetCard(CardItemSO cardSO, InventoryItem inventoryItem)
    {
        this.card = cardSO;
        this.inventoryItem = inventoryItem;
        cardImage.sprite = card.itemImage;
        currentIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
    }

    public void ActiveCardSkill(GameObject player)
    {
        if (card != null)
        {
            Debug.Log("Active");

            card.ActiveCardEffect(player);
            currentIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
            inventoryData.RemoveItem(currentIndex, 1);//consume
            //call update by Action handler
        }
    }

    public void UpdateCardSlot()
    {

        if (card == null)
            return;
        //update

        int tempIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
        Debug.Log("temp: " + tempIndex + " curr: " + currentIndex);
        if (tempIndex != -1)//swapping
        {
            currentIndex = tempIndex;
        }


        this.inventoryItem = inventoryData.GetItemAt(currentIndex);
        this.card = (CardItemSO)this.inventoryItem.item;

        //Debug.Log(inventoryItem.IsEmpty);
        if (inventoryItem.IsEmpty)
            cardImage.sprite = slotDefaultImage;//reset
    }
}
