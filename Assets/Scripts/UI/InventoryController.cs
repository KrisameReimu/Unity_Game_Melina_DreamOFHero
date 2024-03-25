using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public UIItemInventory inventoryUI;

    public int inventorySize = 5;
    private void Awake()
    {
        inventoryUI = GameObject.Find("UI").GetComponentInChildren<UIItemInventory>();
        inventoryUI.InitInventoryUI(inventorySize);
        inventoryUI.Hide();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            if (!inventoryUI.isActiveAndEnabled)
                inventoryUI.Show();
            else 
                inventoryUI.Hide();
        }
    }

    /*
    IEnumerator SetInventoryUI() 
    {
        inventoryUI = GameObject.Find("Inventory").GetComponent<UIItemInventory>();
        yield return new WaitForSeconds(2);
        Debug.Log("Finish");
        inventoryUI.InitInventoryUI(inventorySize);
    }
    */
}
