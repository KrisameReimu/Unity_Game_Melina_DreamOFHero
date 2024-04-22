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

        public void ActiveCardEffect(GameObject character)
        {
            foreach (CardEffectData data in cardEffectList)
            {
                data.cardEffect.CardEffect(character, data.prefab);
            }
        }
    }


    [Serializable]
    public class CardEffectData
    {
        public CardEffectSO cardEffect;
        public GameObject prefab;
        public float value;
    }
}