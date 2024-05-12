using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBoss : Enemy, IBoss
{
    [SerializeField]
    private string bossName = "Crazy Wizard";
    
    private bool isActivated = false;
    private GameObject playerObj;
    //[SerializeField]
    private float speed = 3.5f;
    private Animator anim;
    
    [SerializeField]
    private Vector2 targetPosition;
 
    [SerializeField]
    private float maxHP = 500f;
    [field: SerializeField]
    public float HP { get; private set; }
    [SerializeField]
    private bool isGettingHit = false;
    [SerializeField]
    private GameObject attackPoint;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private LayerMask charactersLayer;
    private int direction = 1;
    [SerializeField]
    private int shootingDamage;
    private bool isStaying = false;
    [SerializeField]
    private float actionCounter =3;
    private bool isDefeated = false;
    private int hitCounter = 5;
    [SerializeField]
    private GameObject earthSpellPrefab;
    Rigidbody2D rb;
    private bool jumped;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip meleeClip;
    [SerializeField]
    private AudioClip jumpClip;
    private BossHpBar hpBar;

    public event Action OnWizardBossDefaet;


    [SerializeField]
    private bool bossRunMode = false;


    void Awake()
    {
        if (GameData.isWizardBossDead && !bossRunMode)
        {
            Destroy(gameObject);
            return;
        }



        playerObj = PlayerController.GetPlayerInstance().gameObject;
        HP = maxHP;
        damage = 15;//melee damage
        shootingDamage = 10;
        isAttacking = false;
        isStaying = true;
        anim = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        //ActivateBoss(); //For test only. This function should be called by other event. 
    }

    // Update is called once per frame
    void Update()
    {
        /*
        targetPosition = playerObj.transform.position;
        ChangeDirection();
        if (Input.GetKeyDown(KeyCode.O))
        {
            EarthSpellAttack();
        }
        return;
        */
        if (!isActivated) return;

        targetPosition = playerObj.transform.position;
        if (isAttacking || isDefeated)
            return;
        ChangeDirection();
        Move();
        PerformAction();
    }

    private void EarthSpellAttack()
    {
        StartCoroutine(EarthSpellSpawn());
    }
    IEnumerator EarthSpellSpawn()
    {
        Vector3 spawnPoint = transform.position + Vector3.right * direction*3f;
        for (int i = 0; i < 7; i++)
        {
            EarthSpell spell =
                Instantiate(earthSpellPrefab, spawnPoint + Vector3.down * 0.6f, Quaternion.identity).GetComponent<EarthSpell>();
            spell.SetDamage(shootingDamage);
            spell.transform.localScale *= new Vector2(direction, 1);
            spawnPoint += Vector3.right * direction * 1;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void PerformAction()
    { 
        actionCounter -= Time.deltaTime;
        if(actionCounter <= 0)
        {
            System.Random rnd = new System.Random();
            actionCounter = rnd.Next(3, 6); //Reset timer to 3-5s

            //System.Random rnd = new System.Random();
            //check distance between player
            if (Vector2.Distance(transform.position, targetPosition) < 2.6)
            {
                isAttacking = true;
                anim.SetTrigger("Melee");            //attack
            }
            else //if(Vector2.Distance(transform.position, targetPosition) < 6)
            {
                isAttacking = true;
                anim.SetTrigger("Shoot");            //attack
            }
            /*
            else
            {
                anim.SetTrigger("Jump");
            */
        }
    }
    
    private void Stay()
    {
        StartCoroutine(StartStaying());
    }

    IEnumerator StartStaying()
    {
        anim.SetBool("Run", false);
        hitCounter = 5;
        isStaying = true;
        isAttacking = false;
        yield return new WaitForSeconds(3);
        isStaying = false;
    }

    private void Move()
    {
        anim.SetBool("Run", false);

        if (isGettingHit || isStaying || isAttacking) return;

        anim.SetBool("Run", true);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, transform.position.y), speed * Time.deltaTime);

        if (!jumped)
        {
            bool needJump = (Vector2.Distance(transform.position, targetPosition) > 15) || (Math.Abs(transform.position.y - targetPosition.y) > 3);
            if (needJump)
                Jump();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isStaying || isAttacking || jumped)
            return;
        if (collision.tag == "Projectile")
        {
            Jump();
        }
    }

    private void Jump()
    {
        anim.SetTrigger("Jump");
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(0, 305), ForceMode2D.Impulse);
        audioSource.PlayOneShot(jumpClip);
        jumped = true;
        StartCoroutine(ToggleJump());
    }

    IEnumerator ToggleJump()
    {
        yield return new WaitForSeconds(8);
        jumped = false;
    }

    private void EndIsGettingHit()
    {
        isGettingHit = false;
        isAttacking = false;
    }

    private void EndAttack()
    {
        isAttacking = false;
        isGettingHit = false;
        //anim.ResetTrigger("Attack");
        anim.ResetTrigger("Hurt");
    }

    private void MeleeAttack()
    {
        isAttacking = true;
        Collider2D[] characters = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, charactersLayer);

        foreach (Collider2D character in characters)
        {
            PlayerController player = character.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.isInvincible)
                    return;
                ChangePlayerHP(player);
                //Instantiate(hitEffectPrefab, attackPoint.transform.position + Vector3.right * 0.5f, Quaternion.identity);
            }
        }
    }
    private void AttackMoveForward()
    {
        rb.velocity =  Vector2.right * direction * 13;
        audioSource.PlayOneShot(meleeClip);
    }
    private void Hurt()
    {
        if (isAttacking) return;
        hitCounter -= 1;
        if(hitCounter <= 0)
        {
            hitCounter = 5;
            isGettingHit = true;
            if (HP > 0)
                anim.SetTrigger("Hurt");
        }
    }

    public override void ChangeHP(float amount)
    {
        if(isDefeated || !isActivated) 
            return; 

        //isGettingHit = true;
        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        Renderer r = gameObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", Color.red);
        r.material.DOColor(Color.white, 0.3f);
        ShowDamageText(amount);

        hpBar.SetHPValue(HP / (float)maxHP);


        
        if (HP > 0)
            Hurt();
        else
        { 
            StartCoroutine(Defeat());
            /*
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.simulated = false;
            */
            /*
            isAttacking = false;
            isDefeated = true;
            anim.SetTrigger("Die");
            */
            //DropCard();
        }
    }

    IEnumerator Defeat()
    {
        OnWizardBossDefaet?.Invoke();

        if (!bossRunMode)
            GameData.isWizardBossDead = true;

        DropItem();
        isAttacking = false;
        isDefeated = true;
        anim.ResetTrigger("Hurt");
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        yield return new WaitForSeconds(3);
        Destroy(hpBar.gameObject);
        Destroy(gameObject);
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

    private void ChangeDirection()
    {
        if (isDefeated) return;

        if (transform.position.x <= targetPosition.x)
            direction = 1;
        else
            direction = -1;
        transform.localScale = new Vector2(direction* Math.Abs(transform.localScale.x), transform.localScale.y);
    }
    public void ActivateBoss()
    {
        if (GameData.isWizardBossDead && !bossRunMode)
        {
            Destroy(gameObject);
            return;
        }


        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.None;
        isActivated = true;
        isStaying = false;
        hpBar = BossHpArea.instance.InitHpBar();
        hpBar.SetBossName(bossName);
    }
}

public interface IBoss
{
    public void ActivateBoss();
}