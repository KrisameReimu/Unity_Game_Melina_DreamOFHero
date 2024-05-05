using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Slime : Enemy
{
    private float speed = 3f;
    private Animator anim;
    public Vector2 originalPosition;
    private Vector2 targetPosition;
    private bool chase = false;
    private float maxHP = 20f;
    public float HP;

    // Start is called before the first frame update
    void Awake()
    {
        HP = maxHP;
        damage = 3;
        isAttacking = false;
        anim = GetComponent<Animator>();
        originalPosition = transform.position;
        targetPosition = originalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        Move();
    }

    private void Move()
    {
        //attack motion
        if (chase && Vector2.Distance(transform.position, targetPosition) != 0)            
            anim.SetTrigger("Attack");
        
        //move
        if(isAttacking)
            transform.position = Vector2.MoveTowards(transform.position,new Vector2(targetPosition.x, transform.position.y), speed*Time.deltaTime);
    }


    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag != "Player" || isAttacking)
            return;
        PlayerController player = c.gameObject.GetComponent<PlayerController>();
        targetPosition = player.transform.position;
        chase = true;
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag != "Player")
            return;
        targetPosition = originalPosition;
        chase = false;
    }

    private void ChangeDirection()
    {
        if (transform.position.x <= targetPosition.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void startAttacking() 
    {
        isAttacking = true;
    }

    public void stoptAttacking()
    {
        isAttacking = false;
    }

    public override void ChangeHP(float amount)
    {
        isAttacking = false;
        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        if (HP > 0)
            anim.SetTrigger("Hurt");

        else
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.simulated = false;
            anim.SetTrigger("Die");
            DropCard();
        }

    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
