using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR // => Ignore from here to next endif if not in editor
using UnityEditorInternal.Profiling.Memory.Experimental;
#endif

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
    private CardItemSO tempCard;
    [SerializeField] 
    private InventoryItem tempInventoryItem;
    [SerializeField]
    private InventorySO inventoryData;
    [SerializeField]
    private List<UICardSlot> cards;
    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private UICardSlotPopUp cardSlotPopUp;
    [SerializeField]
    private static AgentCard acInstance;




    private void Awake()
    {
        if (acInstance == null)
        {
            acInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        if (UI != null)
            return;

        inventoryData.OnInventoryUpdated += UpdateAllCardSlots;

        UI = GameObject.Find("UI");
        Transform cardArea = UI.transform.Find("CardArea");
        foreach (Transform cardSlot in cardArea)
        {
            UICardSlot cardSlotObject = cardSlot.GetComponent<UICardSlot>();
            if (cardSlotObject != null)
            {
                cards.Add(cardSlotObject);
            }
            
        }

        cardSlotPopUp = UI.GetComponentInChildren<UICardSlotPopUp>();
        cardSlotPopUp.OnSlotChosen += SetCard;
    }

    public void ShowCardSlotPopUp(CardItemSO cardSO, InventoryItem inventoryItem)  
    {
        tempCard = cardSO;
        tempInventoryItem = inventoryItem;
        cardSlotPopUp.Toggle(true);

        //cards[0].SetCard(cardSO, inventoryItem);

        /*
        this.card = cardSO;
        this.inventoryItem = inventoryItem;
        cardImage.sprite = card.itemImage;
        currentIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
        */
    }

    public void SetCard(int index) // index = 0, 1, 2, 3
    {
        //check if same card in other slots
        foreach (UICardSlot cardSlot in cards)
        {
            if (cardSlot.GetCurrentIndex() == inventoryData.GetInventoryIndex(tempInventoryItem))
                cardSlot.SetCard(null, InventoryItem.GetEmptyItem());
        }

        cards[index].SetCard(tempCard, tempInventoryItem);
    }

    public bool ActiveCardSkill(int index)
    {
        return cards[index].ActiveCardSkill(gameObject);

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

    //For save & load
    public int[] GetAllCurrentIndex()
    {
        int[] indexes = new int[cards.Count];//4
        for (int i = 0; i < cards.Count; i++)
        {
            indexes[i] = cards[i].GetCurrentIndex();
        }
        Debug.Log(indexes.Length);
        return indexes;
    }
    public void LoadAndEquipAllCards(int[] indexes)
    {
        for (int i = 0; i < indexes.Length; i++) 
        {
            if (indexes[i] == -1)//empty
            {
                this.tempInventoryItem = InventoryItem.GetEmptyItem();
            }
            else
            {
                this.tempInventoryItem = inventoryData.GetItemAt(indexes[i]);
            }
            this.tempCard = (CardItemSO)tempInventoryItem.item;

            SetCard(i);
        }
    }

    void OnDestroy()
    {
        if (acInstance != this)
            return;

        inventoryData.OnInventoryUpdated -= UpdateAllCardSlots;
        cardSlotPopUp.OnSlotChosen -= SetCard;
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
