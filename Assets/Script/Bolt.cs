using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

        /*
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            Debug.Log("Hit");
        }
        */

        Destroy(gameObject);
    }

}
