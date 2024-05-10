using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DitheredFire;

public class WaterBlast : MonoBehaviour
{
    [SerializeField]
    private int damage = 15;
    private int direction;
    private int remainingRound = 4;
    private List<GameObject> hittedObjects;
    private Rigidbody2D rb;
    private bool activated = false;
    private Animator anim;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;

    private void Awake()
    {
        PlayerController player = PlayerController.GetPlayerInstance();
        direction = player.direction;
        transform.position = (Vector2)player.transform.position + new Vector2(direction, 1.7f);
        hittedObjects = new List<GameObject>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if(activated)
            rb.velocity = Vector2.right * direction * 5;
    }
    private void PlayerEffectSound()
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void ActiveWaterBlast()
    {
        activated = true;
    }

    private void EndOfRound()
    {
        remainingRound -= 1;
        //Debug.Log(remainingRound);
        hittedObjects.Clear();

        if (remainingRound <= 0)
        {
            anim.SetTrigger("End");
            activated = false;
        }
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Attack(other);
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        Attack(other);
    }


    private void Attack(Collision2D other)
    {
        if (!activated)
            return;

        if (other.gameObject.tag == "Enemy")
        {
            Enemy e = other.gameObject.GetComponent<Enemy>();
            if (e != null)
            {
                if (hittedObjects.Contains(e.gameObject))//hitted in this round
                    return;

               
                e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
                e.Knockback(25f, -direction);
                hittedObjects.Add(e.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!activated)
            return;

        if (collision.tag == "Trap")
        {
            InteractableTrap trap = collision.GetComponent<InteractableTrap>();
            if (trap == null)
                return;

            IFlame flame = trap as IFlame;
            if (flame != null)
                flame.Extinguish();
        }
    }
}
