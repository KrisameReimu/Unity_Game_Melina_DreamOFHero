using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem {  get; private set; }

    [field: SerializeField]
    public int Quantity = 1;

    

    [SerializeField]    
    private float duration = 0.3f;

    public bool isInteracting = false;

    

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.itemImage;
    }

    public void DestroyItem() 
    {
        GetComponent<Rigidbody2D>().simulated = false;
        StartCoroutine(ItemPickUp());
    }

    private IEnumerator ItemPickUp()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while(currentTime < duration) 
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currentTime/duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
