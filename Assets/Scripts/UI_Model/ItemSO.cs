using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inventory.Model
{
    //[CreateAssetMenu(menuName = "ItemSO/ItemSO")]
    public abstract class ItemSO : ScriptableObject
    {
        public int ID => GetInstanceID();

        [field: SerializeField]
        public bool isStackable { get; set; }


        [field: SerializeField]
        public int maxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string itemName { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string description;

        [field: SerializeField]
        public Sprite itemImage;

        [field: SerializeField]
        public List<ItemParameter> defaultParametersList {  get; set; }
    }

    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public float value;
        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }
}
