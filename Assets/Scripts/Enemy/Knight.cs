using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Enemy
{
    [SerializeField]
    private float speed = 1.5f;
    private Animator anim;
    [SerializeField]
    private Vector2 patrolPointStart;
    [SerializeField]
    private Vector2 patrolPointEnd;
    [SerializeField]
    private Vector2 targetPosition;
    [SerializeField]
    private bool chase = false;
    [SerializeField]
    private float maxHP = 35f;
    [field: SerializeField]
    public float HP {  get; private set; }
    [SerializeField]
    private bool isStaying = false;
    [SerializeField]
    private bool isGettingHit = false;
    [SerializeField]
    private GameObject attackPoint;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private LayerMask charactersLayer;
    [SerializeField]
    private GameObject hitEffectPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        HP = maxHP;
        damage = 7;
        isAttacking = false;
        anim = GetComponent<Animator>();
        patrolPointStart = transform.position; 
        patrolPointEnd = patrolPointStart + Vector2.right * 10;
        targetPosition = patrolPointEnd;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        Move();
    }

    private void Move()
    {
        if (isGettingHit) return;

        anim.SetBool("Run", false);
        anim.SetBool("Walk", false);

        if (isStaying &&! chase)
            return;
        
        if(chase && !isAttacking)
        {
            anim.SetBool("Run", true);
            speed = 3;
        }
        else// isAttacking or Walking
        {
            anim.SetBool("Walk", true);
            speed = 1.5f;
        }

       


        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, transform.position.y), speed * Time.deltaTime);

        if (chase)
        {
            //check distance between player
            if (Vector2.Distance(transform.position, targetPosition) < 2)
            {
                isAttacking = true;
                anim.SetTrigger("Attack");            //attack
            }

            return;
        }

        if (!chase && transform.position.x == patrolPointEnd.x) //patrolling, not chasing, arrived destination
        {
            StartCoroutine(Stay());
        }
    }

 

    IEnumerator Stay()
    {
        anim.SetBool("Walk", false);
        isStaying = true;
        yield return new WaitForSeconds(3);
        isStaying = false;
        //swap destination
        Vector2 tempPosition = patrolPointEnd;
        patrolPointEnd = patrolPointStart;
        patrolPointStart = tempPosition;

        targetPosition = patrolPointEnd;
    }

    private void Attack()
    {
        isAttacking = true;
        Collider2D[] characters = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, charactersLayer);

        foreach(Collider2D character in characters)
        {
            PlayerController player = character.GetComponent<PlayerController>();
            if(player != null)
            {
                if (player.isInvincible) 
                    return;
                ChangePlayerHP(player);
                Instantiate(hitEffectPrefab, attackPoint.transform.position + Vector3.right * 0.5f, Quaternion.identity);
            }
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
        anim.ResetTrigger("Attack");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //no effect
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //no effect
    }

    
    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag != "Player")
            return;
        PlayerController player = c.gameObject.GetComponent<PlayerController>();
        targetPosition = player.transform.position;
        chase = true;
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag != "Player")
            return;
        targetPosition = patrolPointEnd;
        chase = false;
    }
    

    private void ChangeDirection()
    {
        if (transform.position.x <= targetPosition.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void EndIsGettingHit()
    {
        isGettingHit = false;
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }

    public override void ChangeHP(float amount)
    {
        isGettingHit = true;
        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        Renderer r = gameObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", Color.red);
        r.material.DOColor(Color.white, 0.5f);
        ShowDamageText(amount);
        if (!chase)
        {
            chase = true;
            PlayerController player = PlayerController.GetPlayerInstance();
            targetPosition = player.transform.position;
        }

        if (HP > 0)
            anim.SetTrigger("Hurt");
        else
        {
            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.simulated = false;
            
            isAttacking = false;
            anim.SetTrigger("Die");
            DropCard();
        }
    }
}
