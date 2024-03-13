using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 5;//default
    //set the damage value in the awake function in each enemy script
    public virtual void ChangeHP(float amount) { }
    private void OnCollisionEnter2D(Collision2D c)
    {
        AttackPlayer(c);
    }
    private void OnCollisionStay2D(Collision2D c)
    {
        AttackPlayer(c);
    }
    private void AttackPlayer(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                int playerDirection = transform.position.x > player.transform.position.x ? 1 : -1;
                player.ChangeHP(-1 * damage, playerDirection);
            }
        }
    }
}
