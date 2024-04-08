using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private Collider2D _collider;
    private bool playerOnPlatform;
    private Collider2D playerCollider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (playerOnPlatform && Input.GetAxisRaw("Vertical") < 0) 
        {
            //Debug.Log("fall");
            Physics2D.IgnoreCollision(_collider, playerCollider, true);
            StartCoroutine(EnableCollision());
        }
    }

    private IEnumerator EnableCollision() 
    {
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(_collider, playerCollider, false);
    }

    private void SetPlayerOnPlatform(Collision2D c, bool value)
    {
        if(c.gameObject.tag=="Player")
        {
            playerOnPlatform = value;
            playerCollider = c.collider;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision, false);
    }
}
