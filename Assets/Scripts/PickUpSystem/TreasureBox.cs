using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureBox : MonoBehaviour
{
    [SerializeField]
    private GameObject itemPrefab;
    
    [SerializeField]
    private Sprite openedImg;
    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private GameObject prompt;

    private void Awake()
    {
        renderer  = GetComponent<SpriteRenderer>();
        if (GameData.isSeaBoxOpened)
        {
            renderer.sprite = openedImg;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameData.isSeaBoxOpened)
            return;
        


        if (collision.tag == "Player")
        {


            PlayerController player = collision.GetComponent<PlayerController>();
            if (player == null)
                return;


            prompt.SetActive(true);


            if (Input.GetKeyDown(KeyCode.F)) 
            {
                //open
                Instantiate(itemPrefab, player.transform.position+Vector3.up, Quaternion.identity);
                renderer.sprite = openedImg;

                GameData.isSeaBoxOpened = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player == null)
                return;
            prompt.SetActive(false);
        }
    }
}
