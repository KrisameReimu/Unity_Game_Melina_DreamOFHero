using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Range(0, 10f)]
    private int direction = 1;
    public float speed;
    float x_movement;
    //float y_movement;
    public float timeInvincible = 0.5f;
    private bool isInvincible;
    private bool isUsingBurst = false;
    public int maxHP { get; private set; } = 50;
    public int HP;
    public float maxSP { get; private set; } = 20;
    public float SP;
    public float maxEX { get; private set; } = 100;
    public float EX;


    bool isAttacking;
    float attackInterval = 0.5f;
    float attackTimer = 0f;
    private float invincibleTimer;

    bool isJumping = false;
    bool isGettingHurt = false;
    float gettingHurtTimer = 0.3f;

    public GameObject boltPrefab;
    public GameObject burstImpulsePrefab;
    public GameObject barrierPrefab;



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
        SP = maxSP;
        EX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
        Attack();
        Status();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            anim.SetBool("isJump", true);
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
        } 
    }

    //step on ground
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
            return;
        anim.SetBool("isJump", false);
        isJumping = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
            return;
        anim.SetBool("isJump", true);
        isJumping = true;
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

    private void Status() 
    {
        if (SP < maxSP)
            SP += Time.deltaTime;
        UIStatusBar.instance.SetSPValue(SP / (float)maxSP);
        UIStatusBar.instance.SetBurstValue(EX / (float)maxEX);
    }

    private void Burst()
    {
        if (isUsingBurst) 
        {
            EX -= Time.deltaTime*10;
            if(EX <= 0)
                isUsingBurst = false;
        }
        if (EX < maxEX)
            return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isGettingHurt)
                EX -= 30f;//penalty
            isInvincible = true;
            invincibleTimer = EX/10f;
            anim.SetTrigger("burst");
            isAttacking = true;
            attackTimer = 1.2f;
            isUsingBurst = true;
        }

    }

    private void ActiveImpulse()
    {
        GameObject burstImpulse = Instantiate(burstImpulsePrefab, rb.position + new Vector2(direction, 1), Quaternion.Euler(new Vector3(0, 0, 90)));
    }

    private void ActiveBarrier()
    {
        GameObject barrier = Instantiate(barrierPrefab, rb.position + new Vector2(0, 0.8f), Quaternion.identity);
    }

    private void ShootBolt() 
    {
        //make suring the motion was not stopped
        if (!(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("SquatAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("LookUpAttack")))
            return;


        float hight = anim.GetBool("squatDown") ? 1f : 0f;
        float upperAngle = anim.GetBool("isLookUp") ? 1f : 0f;
        GameObject boltObject = Instantiate(boltPrefab, rb.position + new Vector2(direction*(1+0.4f*hight), 1-0.6f*hight+0.4f * upperAngle), Quaternion.Euler(new Vector3(0, 0, 90 + (90+upperAngle*30) * direction)));
        Bolt bolt = boltObject.GetComponent<Bolt>();
        bolt.Shoot(new Vector2(direction, upperAngle*0.4f), 300);
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

    public void increaseEX(float amount, bool hurt) 
    {//amount: the value of damage       hurt: getting hurt or hitting enemy
        if (hurt)
            amount = -1 * amount / maxHP * maxEX * 2;
        else
            amount = amount / maxHP * maxEX / 2;
        EX += amount;
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

            increaseEX(amount, true);
            //PlaySound(damageClip);
        }

        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        UIStatusBar.instance.SetHPValue(HP / (float)maxHP);
        //Debug.Log("HP: " + HP + "/" + maxHP);
    }
}
