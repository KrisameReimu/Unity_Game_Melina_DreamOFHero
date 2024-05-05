using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSlime : Summon
{
    private float speed = 4f;
    private Animator anim;
    private Vector2 targetPostion;
    private GameObject targetObject;

    // Start is called before the first frame update
    void Awake()
    {
        //player = GameObject.Find("Player").GetComponent<PlayerController>();
        //use parent function to set player
        GetPlayer();
        damage = player.playerAtk * 3;

        lifetime = 20;
        isAttacking = false;
        anim = GetComponent<Animator>();
        targetObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        ChangeDirection();
        Move();
    }

    private void Move()
    {
        //Debug.Log(targetObject == null);

        if (targetObject == null)//no targrt
            targetPostion = player.transform.position;
        else
            targetPostion = targetObject.transform.position;
        //attack motion
        if (Vector2.Distance(transform.position, targetPostion) > (targetObject == null ? 2 : 0) )
            anim.SetTrigger("Attack");

        //move
        if (isAttacking)
            transform.position = Vector2.MoveTowards(transform.position, targetPostion, speed * Time.deltaTime);
    }


    private void OnTriggerStay2D(Collider2D c)
    {
        if (targetObject)//check if locked on any target
            return;
        if (c.gameObject.tag != "Enemy" || isAttacking)
            return;

        targetObject = c.gameObject;
    }



    private void ChangeDirection()
    {
        if (transform.position.x <= targetPostion.x)
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

    public override void TimeOut()
    {
        isAttacking = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        anim.SetTrigger("Die");
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
