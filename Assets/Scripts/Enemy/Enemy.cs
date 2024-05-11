using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Enemy : MonoBehaviour
    //Base class of enemies
{
    public int damage = 0;//default no damage
    //set the damage value in the awake function in each enemy script
    public bool isAttacking = true;//default
    //may set it to false if attack animation is not playing
    public GameObject cardPrefab;
    [SerializeField]
    private GameObject damageTextPrefab;




    public virtual void ChangeHP(float amount) { }//pass a negative value to decrease HP
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (isAttacking)
            AttackPlayer(c);
    }
    private void OnCollisionStay2D(Collision2D c)
    {
        if (isAttacking)
            AttackPlayer(c);
    }
    public void AttackPlayer(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                ChangePlayerHP(player);
            }
        }
    }

    public void ChangePlayerHP(PlayerController player)
    {
        int playerDirection = transform.position.x > player.transform.position.x ? 1 : -1;
        player.ChangeHP(-1 * damage, playerDirection);
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

    public void DropItem()
    {
        if (cardPrefab != null)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            GameObject cardItem = Instantiate(cardPrefab, rb.position + new Vector2(0, 0.5f), Quaternion.Euler(new Vector3(0, 0, 30)));
        }
        else
        {
            Debug.Log("This Enemy's Drop Item/Card have not been set yet");
        }
    }

    public void ShowDamageText(float amount)
    {
        amount = Mathf.Abs(amount);
        if (amount == 0)
            return;
        InitDamageTxt(amount.ToString());
    }

    public void ShowDamageText(string text)
    {
        InitDamageTxt(text);
    }

    private void InitDamageTxt(string text)
    {
        GameObject damageText = Instantiate(damageTextPrefab, transform.position + Vector3.up*1.5f, Quaternion.identity);
        damageText.GetComponent<DamageText>().SetText(text);
    }

    public void ShowDamageText(string text, Color _color)
    {
        GameObject damageText = Instantiate(damageTextPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        damageText.GetComponent<DamageText>().SetText(text);
        damageText.GetComponent<TMP_Text>().color = _color;
    }
}
