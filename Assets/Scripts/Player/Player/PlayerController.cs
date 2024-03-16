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
    //float y_movement;
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
        transform.position = (Vector2)rb.position + new Vector2(x_movement, 0) * speed * Time.fixedDeltaTime;
        //rb.MovePosition(position);
    }

    //-----------------------------------------------------------------------------
    //private methods - don't touch


    private void Move() 
    {
        anim.SetBool("isRun", false);
        x_movement = 0;

        if (isAttacking || isGettingHurt)
            return;


        anim.SetBool("squatDown", false);
        anim.SetBool("isLookUp", false);

        if (Input.GetAxisRaw("Horizontal") < 0)
            direction = -1;
        if (Input.GetAxisRaw("Horizontal") > 0)
            direction = 1;
        ChangeDirection();


        if (Input.GetKey(KeyCode.S) && !anim.GetBool("isJump"))
        {
            anim.SetBool("squatDown", true);
            return;
        }

        if (Input.GetKey(KeyCode.W) && !anim.GetBool("isJump"))
        {
            anim.SetBool("isLookUp", true);
            return;
        }


        if (Input.GetAxisRaw("Horizontal") != 0)
        //moving
        {
            x_movement = Input.GetAxis("Horizontal");
            //y_movement = Input.GetAxis("Vertical");

            if (!anim.GetBool("isJump"))
                anim.SetBool("isRun", true);
        }
    }

    private void Jump()
    {
        if (isAttacking || isGettingHurt || isJumping )
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
        Burst();//highest priority
        if (isAttacking)
            return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
        {
            Invoke("ShootBolt", 0.3f);
            anim.SetTrigger("attack");
            isAttacking = true;
            attackTimer = attackInterval;
        }
    }

    private void Burst()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("burst");
            isAttacking = true;
            attackTimer = 1.2f;
        }

    }

    private void ShootBolt() 
    {
        float hight = anim.GetBool("squatDown") ? 1f : 0f;
        float upperAngle = anim.GetBool("isLookUp") ? 1f : 0f;
        GameObject boltObject = Instantiate(boltPrefab, rb.position + new Vector2(direction*(1+0.4f*hight), 1-0.6f*hight+0.4f * upperAngle), Quaternion.Euler(new Vector3(0, 0, 90 + (90+upperAngle*30) * direction)));
        Bolt bolt = boltObject.GetComponent<Bolt>();
        bolt.Shoot(new Vector2(direction, upperAngle*0.3f), 300);
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

        if (amount < 0) //damage
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
        Debug.Log("HP: " + HP + "/" + maxHP);
    }
}
