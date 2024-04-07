using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public float damage = 0;//default no damage
    //set the damage value in the awake function in each enemy script
    public bool isAttacking = true;//default
    //may set it to false if attack animation is not playing
    public float lifetime = 5;
    private bool timeoutTrigger = false;

    public void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0 && !timeoutTrigger)
        {
            TimeOut();
            timeoutTrigger = true;
        }
    }


    private void OnCollisionEnter2D(Collision2D c)
    {
        AttackEnemy(c);
    }
    private void OnCollisionStay2D(Collision2D c)
    {
        AttackEnemy(c);
    }
    public void AttackEnemy(Collision2D other)
    {
        if (!isAttacking)
            return;
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHP(-1 * damage);
                isAttacking = false;
            }
        }
    }

    public virtual void TimeOut() { }
}