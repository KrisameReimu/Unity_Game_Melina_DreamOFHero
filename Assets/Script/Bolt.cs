using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    Rigidbody2D rb;
    public float damage { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damage = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 50.0f)
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
        Destroy(gameObject);

        if (other.gameObject.tag != "Enemy")
            return;
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
        }
        

    }

}
