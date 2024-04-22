using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "ItemSO/CardItemSO")]
    public class CardItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<CardEffectData> cardEffectList = new List<CardEffectData>();
        public string ActionName => "Equip";

        public bool PerformAction(GameObject character, InventoryItem inventoryItem)
        {
            AgentCard cardSystem = character.GetComponent<AgentCard>();
            if(cardSystem != null)
            {
                //cardSystem.SetCard(this, inventoryItem.itemState == null ?
                //    defaultParametersList : inventoryItem.itemState);
                cardSystem.ShowCardSlotPopUp(this, inventoryItem);
                return true;
            }

            return false;
        }

        public bool ActiveCardEffect(GameObject character)
        {
            PlayerController player = character.GetComponent<PlayerController>();
            float totalCost = 0;
            foreach (CardEffectData data in cardEffectList)
            {
                totalCost += data.cost;
            }
            if (!(player.ConsumeSP((int)totalCost)))//not enough SP to consume
            {
                //Debug.Log("Not enough SP");
                return false;
            }
            //else
            foreach (CardEffectData data in cardEffectList)
            {
                data.cardEffect.CardEffect(character, data.prefab);
            }
            return true;
        }
    }


    [Serializable]
    public class CardEffectData
    {
        public CardEffectSO cardEffect;
        public GameObject prefab;
        public float cost;
    }
}