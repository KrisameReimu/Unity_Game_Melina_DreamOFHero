using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedBall : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player")) 
        {
            //Debug.Log("SpikedBall");
            PlayerController player = c.gameObject.GetComponent<PlayerController>();
            player.ChangeHP(-10, 0);
        }
    }
}
