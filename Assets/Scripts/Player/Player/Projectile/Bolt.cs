using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [field: SerializeField]
    public float damage { get; private set; } // = playerAtk
    [SerializeField]
    private Vector2 initPosition;
    private PlayerController player;
    private int direction;
    [SerializeField]
    private AudioClip hitSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //damage = 5f;
        initPosition = transform.position;
        player = PlayerController.GetPlayerInstance();
        direction = player.direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, initPosition) > 10)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector2 direction, float force, float playerAtk)
    {
        damage = playerAtk;
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Bolt Collision with " + other.gameObject);
        anim.SetTrigger("hit");
        rb.simulated = false; //Disable the bolt
        audioSource.PlayOneShot(hitSound);
        if (other.gameObject.tag != "Enemy")
            return;
        Enemy e = other.collider.GetComponent<Enemy>();
        if (e != null)
        {
            e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
            e.Knockback(25f, -direction);

            INonDamagableObject nonDamagableObject = e as INonDamagableObject;
            if (nonDamagableObject == null) // can obtain damage i.e. not shield
            {
                player.IncreaseEX(damage, false);
            }    
        }
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
