using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Enemy
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    Animator animator;
    private bool broken = true;
    public ParticleSystem smokeEffect;
    private AudioSource audioSource;
    public AudioClip fixedClip;

    public float maxHP = 10f;
    public float HP;




    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        HP = maxHP;
        damage = 7;
        smokeEffect = transform.Find("SmokeEffect").gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (!broken)
        {
            return;
        }
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
            //vertical = !vertical;
        }

        if (HP == 0)
            Fix();
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction; ;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction; ;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2D.MovePosition(position);
    }

    

    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(fixedClip);
    }

    public override void ChangeHP(float amount)
    {
        HP += amount;
        HP = Mathf.Clamp(HP, 0, maxHP);
        Debug.Log("HP: "+HP);
    }

    //Override test
    /*
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                Debug.Log("Override");
                int playerDirection = transform.position.x > player.transform.position.x ? 1 : -1;
                player.ChangeHP(-1 * damage, playerDirection);
            }
        }
    }
    */
}