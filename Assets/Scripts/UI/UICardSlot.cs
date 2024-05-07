using Inventory.Model;
using Inventory.UI;
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
    //[SerializeField]
    //private List<ItemParameter> parametersToModify, itemCurrentState;
    [SerializeField]
    private InventoryItem inventoryItem;
    [field: SerializeField]
    private Image cardImage;
    [field: SerializeField]
    private Sprite slotDefaultImage;
    [field: SerializeField]
    public int currentIndex { get; private set; }
    [SerializeField]
    private UIItemInventory inventoryUI;
    [SerializeField]
    private Image popUpSlotImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI quantityText;


    private void Awake()
    {
        cardImage = GetComponent<Image>();
        inventoryUI = transform.root.GetComponentInChildren<UIItemInventory>();
        quantityText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }
    public void SetCard(CardItemSO cardSO, InventoryItem inventoryItem)
    {
        this.card = cardSO;
        this.inventoryItem = inventoryItem;

        if (inventoryItem.IsEmpty)
        {
            cardImage.sprite = slotDefaultImage;//reset
            popUpSlotImage.sprite = slotDefaultImage;
            quantityText.text = "";
            currentIndex = -1;
        }
        else
        {
            cardImage.sprite = card.itemImage;
            popUpSlotImage.sprite = card.itemImage;
            currentIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
            quantityText.text = inventoryItem.quantity.ToString();
        }
  
    }

    public bool ActiveCardSkill(GameObject player)
    {
        if (card != null)
        {
            //Debug.Log("Active");

            bool result = card.ActiveCardEffect(player);
            if(result)//activated effect
            {
                currentIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
                inventoryData.RemoveItem(currentIndex, 1);//consume
                if (inventoryData.GetItemAt(currentIndex).IsEmpty)
                {
                    inventoryUI.ResetSelection();
                }
                

                //call update by Action handler
            }

            return result;
        }
        return false;
    }

    public void UpdateCardSlot()
    {

        if (card == null)
            return;
        //update

        int tempIndex = inventoryData.GetInventoryIndex(this.inventoryItem);
        //Debug.Log("temp: " + tempIndex + " curr: " + currentIndex);
        if (tempIndex != -1)//swapping
        {
            currentIndex = tempIndex;
        }


        this.inventoryItem = inventoryData.GetItemAt(currentIndex);
        this.card = (CardItemSO)this.inventoryItem.item;
        quantityText.text = inventoryItem.quantity.ToString();


        //Debug.Log(inventoryItem.IsEmpty);
        if (inventoryItem.IsEmpty)
        {
            cardImage.sprite = slotDefaultImage;//reset
            popUpSlotImage.sprite = slotDefaultImage;
            quantityText.text = "";
        }

    }
}
