using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    public float damage { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damage = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 10.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Bolt Collision with " + other.gameObject);
        anim.SetTrigger("hit");
        rb.simulated = false; //Disable the bolt
        if (other.gameObject.tag != "Enemy")
            return;
        Enemy e = other.collider.GetComponent<Enemy>();
        if (e != null)
        {
            e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
            int knockbackDirection = transform.position.x > e.transform.position.x ? 1 : -1;
            e.Knockback(25f, knockbackDirection);
        }
    }

    private void Vanish()
    {
        Destroy(gameObject);
    }
}
