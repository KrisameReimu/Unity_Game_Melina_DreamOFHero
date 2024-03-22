using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
    //Base class of enemies
{
    public int damage = 0;//default no damage
    //set the damage value in the awake function in each enemy script
    public bool isAttacking = true;//default
    //may set it to false if attack animation is not playing
    public GameObject cardPrefab;




    public virtual void ChangeHP(float amount) { }
    private void OnCollisionEnter2D(Collision2D c)
    {
        AttackPlayer(c);
    }
    private void OnCollisionStay2D(Collision2D c)
    {
        AttackPlayer(c);
    }
    public void AttackPlayer(Collision2D other)
    {
        if (!isAttacking)
            return;
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

    public void Knockback(float force, int direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.AddForce(new Vector2(direction * -force, 10f), ForceMode2D.Impulse);
        }
    }

    
    /*
    private void OnDestroy()
    {
        if (cardPrefab != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            GameObject cardItem = Instantiate(cardPrefab, rb.position + new Vector2(0, 0.5f), Quaternion.identity);
        }
    }
    
    */

    public void DropCard()
    {
        if (cardPrefab != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            GameObject cardItem = Instantiate(cardPrefab, rb.position + new Vector2(0, 0.5f), Quaternion.Euler(new Vector3(0, 0, 30)));
            rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);

        }
    }
}
