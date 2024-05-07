using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonDashKnight : Summon
{
    [SerializeField]
    private int attackRound = 3;
    [SerializeField]
    private GameObject attackPoint;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private int direction;
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject hitEffectPrefab;




    // Start is called before the first frame update
    void Awake()
    {
        ActiveSummonEffect();
        isAttacking = false;
        GetPlayer();
        damage = player.playerAtk * 5;
        direction = player.direction;
        transform.localScale = new Vector3(player.direction * System.Math.Abs(transform.localScale.x), transform.localScale.y, 1f);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(direction*5, rb.velocity.y);
    }

    private void Jump()
    {
        //Debug.Log("Jump");
        rb.velocity += Vector2.up * 10;
    }

    private void EndOfRound()
    {
        attackRound -= 1;
        if(attackRound <= 0)
        {
            Vanish();
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
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}
