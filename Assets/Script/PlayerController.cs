using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    Animator anim;

    [Range(0, 10f)]
    public float speed;
    float x_movement;
    float y_movement;
    public float timeInvincible = 0.5f;
    bool isInvincible;
    float invincibleTimer;
    public int maxHP { get; private set; } = 50;
    public int HP;
    bool isAttacking;
    float attackInterval = 0.5f;
    float attackTimer = 0f;
    private int direction = 1;
    bool isJumping = false;
    bool isGettingHurt = false;
    float gettingHurtTimer = 0.3f;

    public GameObject boltPrefab;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        HP = maxHP;
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
        Attack();
    }

    private void FixedUpdate()
    {
        Cooldown();
        transform.position = (Vector2)transform.position + new Vector2(x_movement, 0) * speed * Time.fixedDeltaTime;
        //rb.MovePosition(position);
    }

    //-----------------------------------------------------------------------------
    //private methods - don't touch


    private void Move() 
    {
        anim.SetBool("isRun", false);

        if (Input.GetAxisRaw("Horizontal") != 0 && !isAttacking && !isGettingHurt)
        //moving
        {
            x_movement = Input.GetAxis("Horizontal");
            y_movement = Input.GetAxis("Vertical");

            if (Input.GetAxisRaw("Horizontal") < 0)
                direction = -1;
            if (Input.GetAxisRaw("Horizontal") > 0)
                direction = 1;
            ChangeDirection();
            if (!anim.GetBool("isJump"))
                anim.SetBool("isRun", true);
        }
        else {
            x_movement = 0; 
        }
    }

    private void Jump()
    {
        if (isAttacking || isGettingHurt || isJumping)
            return;
        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("isJump"))
        {
            isJumping = true;
            anim.SetBool("isJump", true);
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
        } 
    }

    //step on ground
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
            return;
        anim.SetBool("isJump", false);
        isJumping = false;
    }

    private void Attack() 
    {
        if (isAttacking)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Invoke("ShootBolt", 0.4f);
            anim.SetTrigger("attack");
            isAttacking = true;
            attackTimer = attackInterval;
        }
    }

    private void ShootBolt() 
    {
        GameObject boltObject = Instantiate(boltPrefab, rb.position + new Vector2(direction, 2.5f*transform.localScale.y), Quaternion.Euler(new Vector3(0, 0, 90 + 90 * direction)));
        Bolt bolt = boltObject.GetComponent<Bolt>();
        bolt.Shoot(new Vector2(direction, 0), 300);
    }

    private void Cooldown()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.fixedDeltaTime;
            if (invincibleTimer <= 0)
                isInvincible = false;
        }

        if (isGettingHurt)
        {
            gettingHurtTimer -= Time.fixedDeltaTime;
            if (gettingHurtTimer <= 0)
                isGettingHurt = false;
        }

        if (isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer <= 0)
                isAttacking = false;
        }
        
    }

    private void ChangeDirection()
    {
        transform.localScale = new Vector3(direction * Math.Abs(transform.localScale.x), transform.localScale.y, 1f);
    }

    //-------------------------------------------------------------------------
    //public methods

    public void ChangeHP(int amount, int knockBackDirection)
    {   //knockBackDirection: change the player direction before knockback
        //can be calculated by the following code in the enemyController script
        //int playerDirection = transform.position.x > player.transform.position.x ? 1 : -1;
        if (amount < 0)
        {
            if (isInvincible)
                return;

            int upperForce = anim.GetBool("isJump") ? 0 : 1;
            anim.SetTrigger("hurt");
            isInvincible = true;
            invincibleTimer = timeInvincible;
            isGettingHurt = true;
            gettingHurtTimer = 0.3f;
            direction = knockBackDirection;
            ChangeDirection();
            rb.AddForce(new Vector2(direction*-5f, 3f*upperForce), ForceMode2D.Impulse);
            //PlaySound(damageClip);
            
        }

        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        UIStatusBar.instance.SetHPValue(HP / (float)maxHP);
        //Debug.Log("HP: " + HP + "/" + maxHP);
    }
}
