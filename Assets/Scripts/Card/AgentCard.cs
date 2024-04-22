using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AgentCard : MonoBehaviour
{
    /*
    [SerializeField]
    private CardItemSO card;//current equiping card
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
    */

    [SerializeField]
    private InventorySO inventoryData;
    [SerializeField]
    private List<UICardSlot> cards;
    [SerializeField]
    private GameObject UI;




    private void Awake()
    {
        if (UI != null)
            return;

        inventoryData.OnInventoryUpdated += UpdateAllCardSlots;

        UI = GameObject.Find("UI");
        Transform cardArea = UI.transform.Find("CardArea");
        foreach (Transform cardSlot in cardArea)
        {
            cards.Add(cardSlot.GetComponent<UICardSlot>());
        }
    }

    public void SetCard(CardItemSO cardSO, InventoryItem inventoryItem)  
    {
        cards[0].SetCard(cardSO, inventoryItem);

        /*
        this.card = cardSO;
        this.inventoryItem = inventoryItem;
        cardImage.sprite = card.itemImage;
        currentIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
        */
    }

    public void ActiveCardSkill()
    {
        cards[0].ActiveCardSkill(gameObject);

        /*
        if (card != null)
        {
            card.ActiveCardEffect(gameObject);//player
            currentIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
            inventoryData.RemoveItem(currentIndex, 1);//consume
            //call update by Action handler
        }
        */
    }

    public void UpdateAllCardSlots(Dictionary<int, InventoryItem> inventoryState)
    {
        foreach (UICardSlot cardSlot in cards)
        {
            cardSlot.UpdateCardSlot();
        }

        /*
        if (card == null)
            return;
        //update

        int tempIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
        Debug.Log("temp: "+tempIndex+ " curr: "+ currentIndex);
        if (tempIndex != -1)//swapping
        {
            currentIndex = tempIndex;
        }


        this.inventoryItem = inventoryData.GetItemAt(currentIndex);
        this.card = (CardItemSO)this.inventoryItem.item;

        //Debug.Log(inventoryItem.IsEmpty);
        if (inventoryItem.IsEmpty)
            cardImage.sprite = slotDefaultImage;//reset
        */
    }

    /*
    public void SetCard(CardItemSO cardSO, List<ItemParameter> itemState)
    {
        if(card != null)
        {
            inventoryData.AddItem(card, 1, itemCurrentState);
        }
        this.card = cardSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameter();
    }

    private void ModifyParameter()
    {
        foreach(var parameter in parametersToModify)
        {
            if(itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                float newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue,
                };
            }
        }
    }
    */
}
