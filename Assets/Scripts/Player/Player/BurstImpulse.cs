using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstImpulse : MonoBehaviour
{
    private CircleCollider2D c;
    private float damage;



    // Start is called before the first frame update
    void Awake()
    {
        c = GetComponent<CircleCollider2D>();
        damage = 30f;
    }

    // Update is called once per frame

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Enemy")
            return;
        Enemy e = other.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
            int knockbackDirection = transform.position.x > e.transform.position.x ? 1 : -1;
            e.Knockback(50f, knockbackDirection);
        }
    }
    private void Vanish()
    {
        Destroy(gameObject);
    }
}
