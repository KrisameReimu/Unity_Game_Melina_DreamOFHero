using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedderTest : MonoBehaviour
{
    private PlayerController player;
    private float interval;
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (interval > 0)
                return;
            if(Input.GetKeyDown(KeyCode.F))
            {
                player = collision.gameObject.GetComponent<PlayerController>();
                player.ToggleClimbing(!player.isClimbing);
                interval = 1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<PlayerController>();
            player.ToggleClimbing(false);
        }
    }

    private void Update()
    {
        if(interval>0)
            interval -= Time.deltaTime;

        //for test
        if (Input.GetKeyDown(KeyCode.O))
        {
            player.ChangeHP(-1,1);
        }
    }
}
