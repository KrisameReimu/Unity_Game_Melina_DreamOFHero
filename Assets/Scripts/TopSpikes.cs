using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopSpikes : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            //Debug.Log("TopSpikes");
            PlayerController player = c.gameObject.GetComponent<PlayerController>();
            player.ChangeHP(-1, 0);
        }
    }
}
