using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int maxHP = 50;
    public int HP;
    bool isAttacking;
    float attackInterval = 0.5f;
    float attackTimer = 0f;

    private int direction = 1;
    bool isJumping = false;
    

    public GameObject boltPrefab;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HP = maxHP;
        speed = 5;
        anim = GetComponent<Animator>();
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

        x_movement = Input.GetAxis("Horizontal");
        y_movement = Input.GetAxis("Vertical");

        if (Input.GetAxisRaw("Horizontal") != 0 && !isAttacking)
        //moving
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
                direction = -1;
            if (Input.GetAxisRaw("Horizontal") > 0)
                direction = 1;
            transform.localScale = new Vector3(direction * 0.5f, 0.5f, 1f);

            if (!anim.GetBool("isJump"))
                anim.SetBool("isRun", true);
        }
        else {
            x_movement = 0; 
        }
    }

    private void Jump()
    {
        if (isAttacking)
            return;
        if (Input.GetKeyDown(KeyCode.Space) && !anim.GetBool("isJump"))
        {
            isJumping = true;
            anim.SetBool("isJump", true);
        }
        if (!isJumping)
        {
            return;
        }

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);

        isJumping = false;
    }

    //step on ground
    private void OnTriggerEnter2D(Collider2D other)
    {
        anim.SetBool("isJump", false);
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
        GameObject boltObject = Instantiate(boltPrefab, rb.position + new Vector2(direction, 1.3f), Quaternion.Euler(new Vector3(0, 0, 90 + 90 * direction)));
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

        if (isAttacking)
        {
            attackTimer -= Time.fixedDeltaTime;
            if (attackTimer <= 0)
                isAttacking = false;
        }
        
    }

    //-------------------------------------------------------------------------
    //public methods
    public void ChangeHP(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            anim.SetTrigger("hurt");
            if (direction == 1)
                rb.AddForce(new Vector2(-5f, 3f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(5f, 3f), ForceMode2D.Impulse);
            //PlaySound(damageClip);
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        //Debug.Log("HP: " + HP + "/" + maxHP);
        //UIHealthBar.instance.SetValue(HP / (float)maxHP);
    }
}
