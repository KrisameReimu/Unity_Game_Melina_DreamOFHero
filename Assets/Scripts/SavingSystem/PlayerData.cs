using Inventory;
using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float HP;
    public float[] position;
    public int[,] inventoryData; // index, itemID, quantity
    public int[] equipingCardsIndex;

    public PlayerData(PlayerController player) 
    { 
        HP = player.HP;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        InventoryController inventoryController = player.GetComponent<InventoryController>();
        Dictionary<int, InventoryItem> inventoryContent = inventoryController.GetPlayerInventoryContent();
        inventoryData = new int[inventoryContent.Count, 3];
        //Debug.Log("Size: "+ inventoryContent.Count);

        int currentItemIdx = 0;
        foreach (var item in inventoryContent)//return Dictionary<int, InventoryItem>
        {
            //Debug.Log("Index: " + item.Key);
            //Debug.Log($"ID: {item.Value.item.itemID}  {item.Value.item.itemName}  quantity: {item.Value.quantity}");

            inventoryData[currentItemIdx, 0] = item.Key;//index
            inventoryData[currentItemIdx, 1] = item.Value.item.itemID;
            inventoryData[currentItemIdx, 2] = item.Value.quantity;

            currentItemIdx++;
        }
        /*
        Debug.Log("Saved Inventory Data: ");

        for (int i = 0; i < inventoryData.GetLength(0); i++)
        {
            Debug.Log(inventoryData[i,0]+ " " + inventoryData[i, 1] + " " + inventoryData[i, 2]);
        }
        */
        AgentCard ac = player.GetComponent<AgentCard>();
        equipingCardsIndex = ac.GetAllCurrentIndex();
        Debug.Log($"{equipingCardsIndex[0]} {equipingCardsIndex[1]} {equipingCardsIndex[2]} {equipingCardsIndex[3]}");
    }
}
