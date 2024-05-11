using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    public GameObject Button;   // Button prompt
    public GameObject talkUI;   // Dialogue box
    private bool playerNearby = false;

    private void Start()
    {
        playerNearby = false;
        Button.SetActive(false);
        talkUI.SetActive(false);
        Debug.Log("Start Test.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            Button.SetActive(true);
            Debug.Log("Player is nearby. Button activated.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            Button.SetActive(false);
            talkUI.SetActive(false);
            Debug.Log("Player has left. Button deactivated and dialogue box hidden.");
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.F))
        {
            talkUI.SetActive(true);
            Debug.Log("F key pressed. Dialogue box activated.");
        }
    }
}
