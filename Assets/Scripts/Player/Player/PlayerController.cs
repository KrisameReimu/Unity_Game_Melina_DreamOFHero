using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerInstance {  get; private set; }

    private Rigidbody2D rb;
    private Animator anim;

    [field: SerializeField]
    public int direction { get; private set; } = 1;

    [field: SerializeField]
    public float originalSpeed { get; private set; } = 5;
    [field: SerializeField]
    public float speed { get; private set; }
    float x_movement;
    float y_movement;
    public float timeOfInvincible = 0.5f;
    [field: SerializeField]
    public bool isInvincible { get; private set; }
    private bool isUsingBurst = false;

    public bool isClimbing { get; private set; } = false;

    [field: SerializeField]
    public float maxHP { get; private set; } = 50;
    [field: SerializeField]
    public float HP { get; private set; }
    public float maxSP { get; private set; } = 20;
    [field: SerializeField]
    public float SP { get; private set; }
    public float maxEX { get; private set; } = 100;
    [field: SerializeField]
    public float EX { get; private set; }


    private bool isAttacking;
    private float attackInterval = 0.6f;//time cannot move starting from attack
    private float attackTimer = 0f;//use with attackInterval
    private float normalAttackCooldownInterval = 1.4f;
    private float normalAttackCooldown = 0f;
    private bool isNormalAttackReady = true;
    private float invincibleTimer;

    public bool isJumping { get; private set; } = false;
    private bool isGettingHurt = false;
    private float gettingHurtTimer = 0.3f;
    private bool alive = true;


    public float basicAtk { get; private set; } = 5;
    public float playerAtk { get; private set; }
    public float playerDef { get; private set; } = 0;

    [field: SerializeField]
    public GameObject boltPrefab { get; private set; }
    [field: SerializeField]
    public GameObject burstImpulsePrefab { get; private set; }
    [field: SerializeField]
    public GameObject barrierPrefab { get; private set; }

    [SerializeField]
    private AgentCard cardSystem;
    private AudioSource audioSource;
    public AudioClip oneShotAudioClip;
    public AudioClip hpDecreaseAudioClip;
    private bool initialized = false;

    public event Action OnPlayerDown;//notify if hp < 0
    public static bool isGamePause { get; private set; }  = false;


    private void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        if (initialized)
            return;


        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cardSystem = GetComponent<AgentCard>();
        HP = maxHP;
        speed = originalSpeed;
        SP = maxSP;
        EX = 0;
        playerAtk = basicAtk;
        audioSource = GetComponent<AudioSource>();
        OnPlayerDown += PlayerDown;
        initialized = true;

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!alive || isGamePause) 
            return;
        Jump();
        Move();
        Attack();
        Status();
    }

    private void FixedUpdate()
    {
        if (!alive)
            return;
        Cooldown();

        transform.position = (Vector2)rb.position + new Vector2(x_movement, y_movement) * speed * Time.fixedDeltaTime;
        //rb.MovePosition(position);
    }

    //-----------------------------------------------------------------------------
    //private methods - don't touch


    private void Move()
    {
        anim.SetBool("isRun", false);
        x_movement = 0;
        y_movement = 0;
        if (isAttacking || isGettingHurt)
            return;


        anim.SetBool("squatDown", false);
        anim.SetBool("isLookUp", false);



        if (isClimbing)
        {
            anim.SetBool("climbing", false);
            anim.SetBool("isJump", false);


            x_movement = Input.GetAxis("Horizontal");
            y_movement = Input.GetAxis("Vertical");

            if (x_movement != 0 || y_movement != 0)//moving
                anim.SetBool("climbing", true);

            return;
        }


        if (Input.GetAxisRaw("Horizontal") < 0)
            direction = -1;
        if (Input.GetAxisRaw("Horizontal") > 0)
            direction = 1;
        ChangeDirection();


        if (Input.GetKey(KeyCode.S) && !isJumping)
        {
            anim.SetBool("squatDown", true);
            return;
        }

        if (Input.GetKey(KeyCode.W) && !isJumping)
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
        if (isAttacking || isGettingHurt || isJumping || isClimbing)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            anim.SetBool("isJump", true);
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
        }
    }


    //Objects not for player landing
    string[] jumpDetectionTags = { "Enemy", "Background" };
    //step on ground
    private void OnTriggerStay2D(Collider2D other)
    {
        /*
        if (other != null)
            print("OnTriggerStay2D" + other.name);
        */
        if (jumpDetectionTags.Contains(other.gameObject.tag))
        {
            return;
        }
        anim.SetBool("isJump", false);
        isJumping = false;
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        /*
        if (other != null)
            print("OnTriggerExit2D" + other.name);
        */
        if (jumpDetectionTags.Contains(other.gameObject.tag))
            return;
        anim.SetBool("isJump", true);
        isJumping = true;
    }

    private void Attack()
    {
        Burst();//highest priority
        if (isAttacking || isGettingHurt || isClimbing)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))//normal attack
        {
            if (!isNormalAttackReady)//not ready
                return;

            Invoke("ShootBolt", 0.3f);
            anim.SetTrigger("attack");
            isAttacking = true;
            attackTimer = attackInterval;
            normalAttackCooldown = normalAttackCooldownInterval;
            ConsumeSP(1);
            // Play the one-shot audio clip for the attack sound effect
            if (oneShotAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(oneShotAudioClip, transform.position);
            }

            return;
        }
        CardSkill();
    }

    public void ActiveCardSkill(int index)
    {
        bool activated = cardSystem.ActiveCardSkill(index);

        if(activated)//active animation if used skill only
        {
            anim.SetTrigger("summon");
            isAttacking = true;
            attackTimer = 0.6f;
        }
    }
    
    private void CardSkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            ActiveCardSkill(0);
        }        

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActiveCardSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActiveCardSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ActiveCardSkill(3);
        }
    }
    


    private void Status()
    {
        if (SP < maxSP)
        {
            SP += Time.deltaTime;
            SP = Mathf.Clamp(SP, 0, maxSP);
        }
        UIStatusBar.instance.SetSPValue(SP / (float)maxSP);
        UIStatusBar.instance.SetBurstValue(EX / (float)maxEX);
    }

    private void Burst()
    {
        if (isUsingBurst)
        {
            EX -= Time.deltaTime * 20;
            EX = Mathf.Clamp(EX, 0, maxEX);
            if (EX <= 0)
                isUsingBurst = false;
        }
        if (EX < maxEX)
            return;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isGettingHurt)
                EX -= 30f;//penalty

            isInvincible = true;
            invincibleTimer = EX / 20f;
            anim.SetTrigger("burst");
            isAttacking = true;
            attackTimer = 1.2f;
            isUsingBurst = true;
            ToggleClimbing(false);
            UIStatusBar.instance.HideBurstIcon();

            StartCoroutine(FreezePlayerPosition());
        }
    }

    IEnumerator FreezePlayerPosition()
    {
        rb.velocity = Vector2.zero;
        //rb.simulated = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(1.2f);
        //rb.simulated = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.None;
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
        if (!isAttacking)
            return;


        float hight = anim.GetBool("squatDown") ? 1f : 0f;
        float upperAngle = anim.GetBool("isLookUp") ? 1f : 0f;
        GameObject boltObject = Instantiate(boltPrefab, rb.position + new Vector2(direction * (0.7f + 0.4f * hight), 1 - 0.6f * hight + 0.4f * upperAngle), Quaternion.Euler(new Vector3(0, 0, 90 + (90 + upperAngle * 30) * direction)));
        Bolt bolt = boltObject.GetComponent<Bolt>();
        bolt.Shoot(new Vector2(direction, upperAngle * 0.4f), 300, playerAtk);
    }

    private void Cooldown()//called in fixed update
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

        if (!isNormalAttackReady)
        {
            normalAttackCooldown -= Time.fixedDeltaTime;
            if(normalAttackCooldown <= 0)
                isNormalAttackReady = true;
        }

    }

    public void IncreaseEX(float amount, bool hurt)
    {//amount: the value of damage       hurt: getting hurt or hitting enemy
        if (isUsingBurst)
            return;
        if (hurt)
            amount = -1 * amount / maxHP * maxEX * 2;
        else
            amount = amount / maxHP * maxEX / 2;
        EX += amount;
        EX = Mathf.Clamp(EX, 0, maxEX);

        UIStatusBar.instance.SetBurstValue(EX / (float)maxEX);
        UIStatusBar.instance.ChangeGaugeColor();
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

        if(!alive) return;

        if (amount < 0) //damage
        {
            if (isInvincible)
                return;

            int upperForce = anim.GetBool("isJump") ? 0 : 1;
            anim.SetTrigger("hurt");
            isInvincible = true;
            invincibleTimer = timeOfInvincible;
            isGettingHurt = true;
            gettingHurtTimer = 0.3f;
            isAttacking = false;
            if (isClimbing)
            {//End climbing
                ToggleClimbing(false);
            }
            if (knockBackDirection != 0)
                direction = knockBackDirection;

            rb.AddForce(new Vector2(direction * -5f, 3f * upperForce), ForceMode2D.Impulse);
            ChangeDirection();
            IncreaseEX(amount, true);
            //PlaySound(damageClip);
            // Play the hpDecreaseAudioClip for HP decrease sound effect
            if (hpDecreaseAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(hpDecreaseAudioClip, transform.position);
            }
        }
        
        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        if (HP <= 0)
            OnPlayerDown?.Invoke();
        UIStatusBar.instance.SetHPValue(HP / (float)maxHP);
        //Debug.Log("HP: " + HP + "/" + maxHP);
    }

    public void ChangeSP(int amount)//receive +/- value
    {
        SP += amount;
        SP = Mathf.Clamp(SP, 0, maxSP);
        UIStatusBar.instance.SetSPValue(SP / (float)maxSP);
    }
    public bool ConsumeSP(int amount)//receive a positive value
    {
        if(SP - amount <= 0) 
            return false; //not enough SP to consume

        //else
        ChangeSP(amount * -1);
        return true;
    }

    private void PlayerDown()
    {
        anim.SetTrigger("die");
        alive = false;
        x_movement = 0;
        y_movement = 0;
    }


    public void ToggleClimbing(bool status)
    {
        if (status && isJumping)
        {
            return;
        }

        isClimbing = status;

        //Debug.Log("isClimbing: " + isClimbing);
        if (isClimbing)
        {
            rb.gravityScale = 0;
            speed = 2;
            anim.SetBool("squatDown", false);
            anim.SetBool("isLookUp", false);
            anim.SetBool("climb", true);
        }
        else
        {
            rb.gravityScale = 5;
            speed = originalSpeed;
            anim.SetBool("climb", false);
            anim.SetBool("climbing", false);
        }
    }



    public void MoveToNewPosition(Vector2 position)
    {
        rb.position = position;
        transform.position = position;
    }

    public void InitPlayerData(float HP, float SP, float EX, Vector2 position)
    {
        this.HP = HP;
        this.SP = SP;
        this.EX = EX;
        ChangeHP(0, 0);
        IncreaseEX(0, false);
        MoveToNewPosition(position);
    }

    public static PlayerController GetPlayerInstance()
    {
        PlayerController pc = playerInstance;
        if (pc == null)
        {
            pc = GameObject.Find("Player").GetComponent<PlayerController>();
        }
        return pc;
    }

    public void SetHP(float value)
    {
        HP = value;
        UIStatusBar.instance.SetHPValue(HP / (float)maxHP);
    }

    public static void SetIsGamePause(bool status)
    {
        isGamePause = status;
    }

    public void SpeedUp(float effectTime)
    {
        StartCoroutine(StartSpeedUp(effectTime));    
    }
    IEnumerator StartSpeedUp(float effectTime)
    {
        speed *= 1.5f;
        yield return new WaitForSeconds(effectTime);
        speed /= 1.5f;
    }
}
