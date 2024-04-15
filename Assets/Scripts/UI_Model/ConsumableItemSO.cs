using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{

    [CreateAssetMenu(menuName = "ItemSO/ConsumableItemSO")]
    public class ConsumableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();

        public string ActionName => "Consume";

        public bool PerformAction(GameObject character)
        {
            foreach (ModifierData data in modifiersData)
            {
                data.statModifier.ItemEffect(character, data.value);
            }
            return true;
        }
    }

    public interface IDestroyableItem
    { 
    }

    public interface IItemAction
    {
        public string ActionName { get; }
        bool PerformAction(GameObject character);
    }

    [Serializable]
    public class ModifierData
    {
        public PlayerStatModifierSO statModifier;
        public float value;
    }
}