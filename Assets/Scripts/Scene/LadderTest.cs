using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTest : MonoBehaviour
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
                Debug.Log("F");
                player = collision.gameObject.GetComponent<PlayerController>();
                player.ToggleClimbing(!player.isClimbing);
                interval = 0.5f;
            }

            if(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("W");
                if (player.isClimbing)
                    return;

                player = collision.gameObject.GetComponent<PlayerController>();
                player.ToggleClimbing(true);
                interval = 0.5f;
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
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            player.ChangeHP(-1,1);
        }
        */
    }
}
