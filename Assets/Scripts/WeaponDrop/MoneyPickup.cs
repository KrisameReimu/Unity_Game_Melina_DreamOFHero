using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    public int cashValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            IInventory inventory = other.GetComponent<IInventory>();

            if (inventory != null)
            {
                inventory.Money = inventory.Money + cashValue;
                print("Player inventory has " + inventory.Money + " money in it.");
                gameObject.SetActive(false);
            }
        }
    }
}