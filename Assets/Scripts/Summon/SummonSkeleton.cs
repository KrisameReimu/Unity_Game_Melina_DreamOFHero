using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkeleton : Summon
{
    private float speed = 2f;
    private Animator anim;
    private Vector2 targetPosition;
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private GameObject attackPoint;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private LayerMask enemyLayer;
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject hitEffectPrefab;
    [SerializeField]
    private bool stay = false;
    // Start is called before the first frame update
    void Awake()
    {
        ActiveSummonEffect();
        GetPlayer();
        damage = player.playerAtk * 2;

        lifetime = 30;
        isAttacking = false;
        anim = GetComponent<Animator>();
        targetObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (!isAttacking)
        {
            ChangeDirection();
        }
        Move();
        if (stay)
        {
            if (Vector2.Distance(transform.position, player.transform.position) > 2.2)
                stay = false;
        }
    }
    private void Move()
    {
        if (isAttacking || stay)
            return;
        //Debug.Log(targetObject == null);

        if (targetObject == null)//no targrt
            targetPosition = player.transform.position;
        else
            targetPosition = targetObject.transform.position;

        anim.SetBool("Walk", true);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, transform.position.y), speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPosition) < 2.2)
        {
            if (targetPosition == (Vector2)player.transform.position)//if following player
            {
                //Debug.Log("Idle");
                anim.SetBool("Walk", false);//idle
                stay = true;
            }
            else
            {
                //else attack enemy
                //Debug.Log("Attack");

                anim.SetTrigger("Attack");
                isAttacking = true;
            }
        }
    }

    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemyObj in enemies)
        {
            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
                int knockbackDirection = transform.position.x > enemy.transform.position.x ? 1 : -1;
                enemy.Knockback(25f, knockbackDirection);
                Instantiate(hitEffectPrefab, attackPoint.transform.position + Vector3.right * 0.5f, Quaternion.identity);
                targetObject = null;//reset target
            }
        }
    }

    private void ChangeDirection()
    {
        if (transform.position.x <= targetPosition.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    private void OnTriggerStay2D(Collider2D c)
    {
        if (targetObject)//check if locked on any target
            return;
        if (c.transform.root.tag != "Enemy" || isAttacking)
            return;

        IRespawnable enemy = c.transform.root.GetComponent<Enemy>() as IRespawnable;
        if (enemy != null)
        {
            if (enemy.isDead)
                return;
        }

        targetObject = c.transform.root.gameObject;
        stay = false;
    }

    private void AttackMoveForward()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = (transform.rotation.y == 0 ? Vector2.right : Vector2.left) * 5;
    }

    private void EndAttack()
    {
        isAttacking = false;
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

    public override void TimeOut()
    {
        Vanish();
    }
}
