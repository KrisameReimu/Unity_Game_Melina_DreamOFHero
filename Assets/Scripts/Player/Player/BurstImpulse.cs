using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstImpulse : MonoBehaviour
{
    private CircleCollider2D c;
    private float damage;
    private Vector2 offset;
    private PlayerController player;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip hitClip;



    // Start is called before the first frame update
    void Awake()
    {
        c = GetComponent<CircleCollider2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        damage = player.playerAtk * 6;
        offset = transform.position - player.transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = (Vector2)player.transform.position + offset;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag != "Enemy")
            return;
        Enemy e = other.gameObject.GetComponent<Enemy>();
        if (e != null)
        {
            e.ChangeHP(-1 * damage); //call the function to decrease enemies' HP
            int knockbackDirection = transform.position.x > e.transform.position.x ? 1 : -1;
            e.Knockback(75f, knockbackDirection);
            audioSource.PlayOneShot(hitClip);
        }
    }
    private void Vanish()
    {
        Destroy(gameObject);
    }
}
