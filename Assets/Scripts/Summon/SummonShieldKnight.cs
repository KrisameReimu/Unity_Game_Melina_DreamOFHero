using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EarthSpell;

public class SummonShieldKnight : Summon
{
    private Animator anim;
    [SerializeField]
    private float guardCounter = 0;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip guardClip;

    void Awake()
    {
        ActiveSummonEffect();
        damage = 0;
        lifetime = 10;
        isAttacking = true;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        GetPlayer();
        transform.localScale = new Vector3(player.direction * System.Math.Abs(transform.localScale.x), transform.localScale.y, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        ResetGuardCounter();
    }

    private void ResetGuardCounter()
    {
        if(!isAttacking && guardCounter > 0)
        {
            guardCounter -= Time.deltaTime;
            if (guardCounter <= 0)
                isAttacking = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        SetGuardCounter();
        Guard(c);
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        SetGuardCounter();
        Guard(c);
    }

    private void Guard(Collision2D other) 
    {
        if(other.gameObject.tag == "EnemyProjectile")
        {
            IEnemyProjectile enemyProjectile = other.gameObject.GetComponent<IEnemyProjectile>();
            if(enemyProjectile != null) 
            {
                enemyProjectile.IsGuarded();
                audioSource.PlayOneShot(guardClip);
                GuardEffect();

                lifetime -= enemyProjectile.damage * 0.1f;
                ChangeDirection(other.transform.position);
                return;
            }
        }


        if (!isAttacking)
            return;
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ChangeHP(-1 * damage);
                int knockbackDirection = transform.position.x > enemy.transform.position.x ? 1 : -1;
                enemy.Knockback(25f, knockbackDirection);
                isAttacking = false;
                GuardEffect();
                lifetime -= enemy.damage * 0.1f;
                ChangeDirection(other.transform.position);
            }
            audioSource.PlayOneShot(guardClip);
        }
    }

    private void GuardEffect()
    {
        Renderer r = gameObject.GetComponent<Renderer>();
        r.material.SetColor("_Color", Color.yellow);
        r.material.DOColor(Color.white, 0.5f);
    }

    private void ChangeDirection(Vector2 targetPosition)
    {
        if (transform.position.x <= targetPosition.x)
            transform.localScale = new Vector3(1 * System.Math.Abs(transform.localScale.x), transform.localScale.y, 1f);
        else
            transform.localScale = new Vector3(-1 * System.Math.Abs(transform.localScale.x), transform.localScale.y, 1f);
    }

    private void SetGuardCounter()
    {
        if(isAttacking) 
            guardCounter = 1;
    }

    public override void TimeOut()
    {
        /*
        isAttacking = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        anim.SetTrigger("Die");
        */
        Vanish();
    }
}
