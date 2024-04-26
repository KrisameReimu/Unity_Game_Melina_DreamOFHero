using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_collider : MonoBehaviour
{
    
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit");
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.ChangeHP(-1, 0);
        }
    }
   
}
