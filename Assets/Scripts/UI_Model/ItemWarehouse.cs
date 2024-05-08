using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemSO/ItemWarehouse")]
public class ItemWarehouse : ScriptableObject
{
    [SerializeField]
    private List<ItemSO> itemList;

    public ItemSO GetItemSO(int itemID) //receive item ID
    {
        return itemList[itemID];
    }
}
