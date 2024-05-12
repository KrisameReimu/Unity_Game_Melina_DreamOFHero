using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadExecutionerBoss : Enemy, IBoss, IUndead
{
    private string bossName = "Undead Executioner";
    
    private bool isActivated = false;
    private GameObject playerObj;
    //[SerializeField]
    private float speed = 3f;
    private Animator anim;

    [SerializeField]
    private Vector2 targetPosition;

    [SerializeField]
    private float maxHP = 700f;
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
    private int summonDamage;
    private bool isStaying = false;
    [SerializeField]
    private float actionCounter = 3;
    private bool isDefeated = false;
    private int hitCounter = 4; //no of hit before knockback
    [SerializeField]
    private GameObject summonPrefab;
    Rigidbody2D rb;
    private bool faded;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip meleeClip;
    [SerializeField]
    private AudioClip teleportClip;
    
    private BossHpBar hpBar;
    [SerializeField]
    private GameObject hitEffectPrefab;


    public event Action OnUndeadBossDefeat;

    [SerializeField]
    private bool bossRunMode = false;



    void Awake()
    {

        if(GameData.isUndeadBossDead && !bossRunMode)
        {
            Destroy(gameObject);
            return;
        }


        playerObj = PlayerController.GetPlayerInstance().gameObject;
        HP = maxHP;
        damage = 20;//melee damage
        summonDamage = 10;
        isAttacking = false;
        isStaying = true;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector2.Distance(transform.position, targetPosition));
        if (!isActivated) return;

        targetPosition = playerObj.transform.position+Vector3.up*0.8f;


        if (isAttacking || isDefeated)
            return;
        ChangeDirection();
        Move();
        PerformAction();
    }

    private void SummonSpell()
    {
        StartCoroutine(SummonSpawn());
    }
    IEnumerator SummonSpawn()
    {
        Vector3 spawnPoint = transform.position + Vector3.left * direction  + Vector3.up*3;
        for (int i = 0; i < 4; i++)
        {
            UndeadSummon summon =
                Instantiate(summonPrefab, spawnPoint, Quaternion.identity).GetComponent<UndeadSummon>();
            summon.SetDamage(summonDamage);
            spawnPoint += Vector3.left * direction + Vector3.down;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void PerformAction()
    {
        actionCounter -= Time.deltaTime;
        if (actionCounter <= 0)
        {

            //Reset timer
            System.Random rnd = new System.Random();
            actionCounter = rnd.Next(2, 5);

            rb.velocity = Vector3.zero;

            //System.Random rnd = new System.Random();
            //check distance between player
            if (Vector2.Distance(transform.position, targetPosition) < 5f)
            {
                
                anim.SetTrigger("Melee");            //attack
            }
            else if(Vector2.Distance(transform.position, targetPosition) < 8)
            {
                if(actionCounter <4) // 2 or 3
                //isAttacking = true;
                    anim.SetTrigger("Summon");            //attack
                else
                    anim.SetTrigger("Teleport");
            }
            else
            {
                anim.SetTrigger("Teleport");
            }
            
            isAttacking = true;

        }
    }

    private void Stay()
    {
        StartCoroutine(StartStaying());
    }

    IEnumerator StartStaying()
    {
        anim.SetBool("Move", false);
        hitCounter = 4;
        isStaying = true;
        isAttacking = false;
        yield return new WaitForSeconds(2);
        isStaying = false;
    }

    private void Move()
    {
        anim.SetBool("Move", false);

        if (isGettingHit || isStaying || isAttacking) return;

        anim.SetBool("Move", true);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        /*
        if (!jumped)
        {
            bool needJump = (Vector2.Distance(transform.position, targetPosition) > 15) || (Math.Abs(transform.position.y - targetPosition.y) > 3);
            if (needJump)
                Jump();
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isStaying || isAttacking || faded)
            return;
        if (collision.tag == "Projectile")
        {
            StartCoroutine(FadeDodge());
        }
    }

    

    IEnumerator FadeDodge()
    {
        faded = true;
        anim.SetTrigger("Fade");
        yield return new WaitForSeconds(10);
        faded = false;
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
        rb.velocity = Vector2.zero;
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
                Instantiate(hitEffectPrefab, attackPoint.transform.position + Vector3.right * 0.5f, Quaternion.identity);
            }
        }
    }
    private void AttackMoveForward()
    {
        //Aim player
        rb.velocity = (targetPosition - (Vector2)transform.position).normalized * 10;
        PlayMeleeClip();
    }

    private void PlayMeleeClip()
    {
        audioSource.PlayOneShot(meleeClip);

    }
    private void Hurt()
    {
        if (isAttacking) return;
        hitCounter -= 1;
        if (hitCounter <= 0)
        {
            hitCounter = 5;
            isGettingHit = true;
            if (HP > 0)
                anim.SetTrigger("Hurt");
        }
    }

    private void Teleport()
    {
        PlayerController player = playerObj.GetComponent<PlayerController>();
        transform.position = targetPosition + Vector2.right*player.direction*1.5f+Vector2.up;
    }
    private void PlayTeleportClip()
    {
        audioSource.PlayOneShot(teleportClip);
    }

    public void Purify(float value)
    {
        ChangeHP(value*2);
        ShowDamageText("Purify", Color.yellow);
    }


    public override void ChangeHP(float amount)
    {
        if (isDefeated || !isActivated)
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
        }
    }

    IEnumerator Defeat()
    {
        if(!bossRunMode)
            GameData.isUndeadBossDead = true;

        OnUndeadBossDefeat?.Invoke();

        DropBossCard();
        anim.SetBool("Die", true);
        isAttacking = false;
        isDefeated = true;
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
        transform.localScale = new Vector2(direction * Math.Abs(transform.localScale.x), transform.localScale.y);
    }
    public void ActivateBoss()
    {
        if (GameData.isUndeadBossDead && !bossRunMode)
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
